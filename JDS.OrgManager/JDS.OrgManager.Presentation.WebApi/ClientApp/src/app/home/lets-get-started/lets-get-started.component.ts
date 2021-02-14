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
