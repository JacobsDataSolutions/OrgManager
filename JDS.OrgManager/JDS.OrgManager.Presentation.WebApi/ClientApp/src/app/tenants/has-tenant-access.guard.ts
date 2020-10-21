import { Injectable } from "@angular/core";
import {
    CanActivate,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
    Router
} from "@angular/router";
import { Observable } from "rxjs";
import { tap, switchMap, first } from "rxjs/operators";
import { TenantClient } from "../shared/nswag";

@Injectable({
    providedIn: "root"
})
export class HasTenantAccessGuard implements CanActivate {
    constructor(private tenantClient: TenantClient, private router: Router) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        if (route.paramMap.has("slug")) {
            const slug = route.paramMap.get("slug");
            const isAuthorized = this.tenantClient
                .getTenantIdFromSlug(slug)
                .pipe(
                    first(),
                    switchMap((tenantId: number) => {
                        return this.tenantClient.getHasTenantAccess(tenantId);
                    }),
                    tap((auth) => {
                        this.handleAuthorization(auth, state);
                    })
                );
            return isAuthorized;
        }
        return false;
    }

    private handleAuthorization(
        isAuthorized: boolean,
        state: RouterStateSnapshot
    ) {
        if (!isAuthorized) {
            this.router.navigate(["/unauthorized"], {
                queryParams: {
                    ["returnUrl"]: state.url
                }
            });
        }
    }
}
