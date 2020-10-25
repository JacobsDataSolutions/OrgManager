// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
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
