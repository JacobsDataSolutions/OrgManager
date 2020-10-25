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
