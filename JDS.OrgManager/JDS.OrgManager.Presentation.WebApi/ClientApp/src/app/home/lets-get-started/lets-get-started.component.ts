// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { ApplicationPaths } from "../../../api-authorization/api-authorization.constants";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";

@Component({
    selector: "om-lets-get-started",
    templateUrl: "./lets-get-started.component.html",
    styleUrls: ["./lets-get-started.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class LetsGetStartedComponent implements OnInit {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;

    constructor() {}

    ngOnInit(): void {}

    employerRegisterClick() {
        this.redirectToRegister("Employer");
    }

    employeeRegisterClick() {
        this.redirectToRegister("Employee");
    }

    private redirectToRegister(registrationUserType: string): any {
        this.redirectToApiAuthorizationPath(
            `${ApplicationPaths.IdentityRegisterPath}?returnUrl=${encodeURI("/" + ApplicationPaths.Login)}` +
                (registrationUserType ? `&registrationUserType=${registrationUserType}` : "")
        );
    }

    private redirectToApiAuthorizationPath(apiAuthorizationPath: string) {
        // It's important that we do a replace here so that when the user hits the back arrow on the
        // browser they get sent back to where it was on the app instead of to an endpoint on this
        // component.
        const redirectUrl = `${window.location.origin}${apiAuthorizationPath}`;
        window.location.replace(redirectUrl);
    }
}
