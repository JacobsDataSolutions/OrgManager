// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit } from "@angular/core";
import { AuthenticationResultStatus, AuthorizeService } from "../authorize.service";
import { BehaviorSubject } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { take } from "rxjs/operators";
import { LogoutActions, ApplicationPaths, ReturnUrlType } from "../api-authorization.constants";

// The main responsibility of this component is to handle the user's logout process.
// This is the starting point for the logout process, which is usually initiated when a
// user clicks on the logout button on the LoginMenu component.
@Component({
    selector: "app-logout",
    templateUrl: "./logout.component.html",
    styleUrls: ["./logout.component.css"]
})
export class LogoutComponent implements OnInit {
    public message = new BehaviorSubject<string>(null);

    constructor(private authorizeService: AuthorizeService, private activatedRoute: ActivatedRoute, private router: Router) {}

    async ngOnInit() {
        const action = this.activatedRoute.snapshot.url[1];
        switch (action.path) {
            case LogoutActions.Logout:
                // JDS
                await this.logout(this.getReturnUrl());
                //  if (!!window.history.state.local) {
                //    await this.logout(this.getReturnUrl());
                //  } else {
                //    // This prevents regular links to <app>/authentication/logout from triggering a logout
                //    this.message.next('The logout was not initiated from within the page.');
                //  }
                break;
            case LogoutActions.LogoutCallback:
                await this.processLogoutCallback();
                break;
            case LogoutActions.LoggedOut:
                this.message.next("You successfully logged out!");
                //Jake
                await this.navigateToReturnUrl("/");
                break;
            default:
                throw new Error(`Invalid action '${action}'`);
        }
    }

    private async logout(returnUrl: string): Promise<void> {
        const state: INavigationState = { returnUrl };
        const isauthenticated = await this.authorizeService.isAuthenticated().pipe(take(1)).toPromise();
        if (isauthenticated) {
            const result = await this.authorizeService.signOut(state);
            switch (result.status) {
                case AuthenticationResultStatus.Redirect:
                    break;
                case AuthenticationResultStatus.Success:
                    await this.navigateToReturnUrl(returnUrl);
                    break;
                case AuthenticationResultStatus.Fail:
                    this.message.next(result.message);
                    break;
                default:
                    throw new Error("Invalid authentication result status.");
            }
        } else {
            this.message.next("You successfully logged out!");
        }
    }

    private async processLogoutCallback(): Promise<void> {
        const url = window.location.href;
        const result = await this.authorizeService.completeSignOut(url);
        switch (result.status) {
            case AuthenticationResultStatus.Redirect:
                // There should not be any redirects as the only time completeAuthentication finishes
                // is when we are doing a redirect sign in flow.
                throw new Error("Should not redirect.");
            case AuthenticationResultStatus.Success:
                await this.navigateToReturnUrl(this.getReturnUrl(result.state));
                break;
            case AuthenticationResultStatus.Fail:
                this.message.next(result.message);
                break;
            default:
                throw new Error("Invalid authentication result status.");
        }
    }

    private async navigateToReturnUrl(returnUrl: string) {
        await this.router.navigateByUrl(returnUrl, {
            replaceUrl: true
        });
    }

    private getReturnUrl(state?: INavigationState): string {
        const fromQuery = (this.activatedRoute.snapshot.queryParams as INavigationState).returnUrl;
        // If the url is coming from the query string, check that is either
        // a relative url or an absolute url
        if (fromQuery && !(fromQuery.startsWith(`${window.location.origin}/`) || /\/[^\/].*/.test(fromQuery))) {
            // This is an extra check to prevent open redirects.
            throw new Error("Invalid return url. The return url needs to have the same origin as the current page.");
        }
        return (state && state.returnUrl) || fromQuery || ApplicationPaths.LoggedOut;
    }
}

interface INavigationState {
    [ReturnUrlType]: string;
}
