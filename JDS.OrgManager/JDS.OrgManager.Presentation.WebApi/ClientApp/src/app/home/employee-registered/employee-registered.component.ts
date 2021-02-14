import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { Router } from "@angular/router";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";

@Component({
    selector: "om-employee-registered",
    templateUrl: "./employee-registered.component.html",
    styleUrls: ["./employee-registered.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class EmployeeRegisteredComponent implements OnInit {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    constructor(private router: Router) {}

    ngOnInit(): void {}

    onHomeClick() {
        this.router.navigate(["/"]);
    }
}
