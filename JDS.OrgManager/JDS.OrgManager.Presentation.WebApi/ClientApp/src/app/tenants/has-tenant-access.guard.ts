// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
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
