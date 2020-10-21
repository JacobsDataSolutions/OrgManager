import { Injectable } from "@angular/core";
import {
    CanActivate,
    ActivatedRouteSnapshot,
    RouterStateSnapshot,
    Router
} from "@angular/router";
import { Observable, of } from "rxjs";
import { tap, switchMap, first } from "rxjs/operators";
import { CustomerClient, UserClient } from "../shared/nswag";

@Injectable({
    providedIn: "root"
})
export class HasCustomerAccessGuard implements CanActivate {
    constructor(
        private customerClient: CustomerClient,
        private userClient: UserClient,
        private router: Router
    ) {}

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        const isAuthorized: Observable<boolean> = this.userClient
            .getUserStatus()
            .pipe(
                first(),
                switchMap((userStatus) => {
                    if (
                        userStatus.isCustomer &&
                        !userStatus.hasProvidedCustomerInformation &&
                        !state.url.endsWith("customer")
                    ) {
                        this.router.navigate(["/customer"], {
                            queryParams: {
                                ["returnUrl"]: state.url
                            }
                        });
                    } else if (!userStatus.isCustomer) {
                        this.router.navigate(["/unauthorized"], {
                            queryParams: {
                                ["returnUrl"]: state.url
                            }
                        });
                    }
                    return of(userStatus.isCustomer);
                })
            );
        return isAuthorized;
    }
}
