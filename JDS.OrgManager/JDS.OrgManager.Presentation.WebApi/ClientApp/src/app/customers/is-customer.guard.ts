import { Injectable, OnDestroy } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from "@angular/router";
import { Observable, of, Subject } from "rxjs";
import { TenantService } from "../tenants/tenant.service";
import { tap, switchMap, takeUntil, map } from "rxjs/operators";
import { UserClient, UserStatusViewModel } from "../shared/nswag";

@Injectable({
    providedIn: "root"
})
export class IsCustomerGuard implements CanActivate, OnDestroy {
    private ngUnsubscribe = new Subject();

    constructor(private tenantService: TenantService, private router: Router, private userClient: UserClient) {}

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        return this.userClient.getUserStatus(null).pipe(
            map((userStatus: UserStatusViewModel) => {
                if (!userStatus.isCustomer) {
                    return false;
                }
                return true;
            }),
            tap((isAuthorized) => {
                this.handleAuthorization(isAuthorized, state);
            }),
            takeUntil(this.ngUnsubscribe)
        );
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
