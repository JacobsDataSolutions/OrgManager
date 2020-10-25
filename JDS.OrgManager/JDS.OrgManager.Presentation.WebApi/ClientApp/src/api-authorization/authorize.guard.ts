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
import { AuthorizeService } from "./authorize.service";
import { tap } from "rxjs/operators";
import {
    ApplicationPaths,
    QueryParameterNames
} from "./api-authorization.constants";

@Injectable({
    providedIn: "root"
})
export class AuthorizeGuard implements CanActivate {
    constructor(private authorize: AuthorizeService, private router: Router) {}
    canActivate(
        _next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {
        return this.authorize
            .isAuthenticated()
            .pipe(
                tap((isAuthenticated) =>
                    this.handleAuthorization(isAuthenticated, state)
                )
            );
    }

    private handleAuthorization(
        isAuthenticated: boolean,
        state: RouterStateSnapshot
    ) {
        if (!isAuthenticated) {
            this.router.navigate(ApplicationPaths.LoginPathComponents, {
                queryParams: {
                    [QueryParameterNames.ReturnUrl]: state.url
                }
            });
        }
    }
}
