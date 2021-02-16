import { Injectable, OnDestroy } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable, of, Subject } from "rxjs";
import { TenantService } from "../tenants/tenant.service";
import { tap, switchMap, takeUntil } from "rxjs/operators";
import { UserClient, UserStatusViewModel } from "../shared/nswag";

@Injectable({
    providedIn: "root"
})
export class IsEmployeeGuard implements CanActivate, OnDestroy {
    private ngUnsubscribe = new Subject();

    constructor(private tenantService: TenantService, private router: Router, private userClient: UserClient) {}

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        if (route.paramMap.has("slug")) {
            return this.tenantService.getCachedTenantIdFromSlug(route.paramMap.get("slug")).pipe(
                switchMap((tenantId: number) => {
                    return this.userClient.getUserStatus(tenantId);
                }),
                switchMap((userStatus: UserStatusViewModel) => {
                    if (!userStatus.isApprovedEmployee) {
                        return of(false);
                    }
                    return of(true);
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
