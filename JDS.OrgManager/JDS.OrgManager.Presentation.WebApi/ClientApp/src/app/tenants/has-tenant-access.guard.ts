import { Injectable, OnDestroy } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable, Subject } from "rxjs";
import { TenantService } from "./tenant.service";
import { tap, switchMap, takeUntil } from "rxjs/operators";
import { TenantClient } from "../shared/nswag";

@Injectable({
    providedIn: "root"
})
export class HasTenantAccessGuard implements CanActivate, OnDestroy {
    private ngUnsubscribe = new Subject();

    constructor(private tenantService: TenantService, private router: Router, private tenantClient: TenantClient) {}

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        if (route.paramMap.has("slug")) {
            return this.tenantService.getCachedTenantIdFromSlug(route.paramMap.get("slug")).pipe(
                switchMap((tenantId: number) => {
                    return this.tenantClient.getHasTenantAccess(tenantId);
                }),
                tap((isAuthorized) => {
                    this.handleAuthorization(isAuthorized, state);
                }),
                takeUntil(this.ngUnsubscribe)
            );
        }
        return false;
    }

    private handleAuthorization(isAuthorized: boolean, state: RouterStateSnapshot) {
        if (!isAuthorized) {
            this.router.navigate(["/unauthorized"], {
                queryParams: {
                    ["returnUrl"]: state.url
                }
            });
        }
    }
}
