import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";

@Component({
    selector: "om-customer-home",
    templateUrl: "./customer-home.component.html",
    styleUrls: ["./customer-home.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class CustomerHomeComponent implements OnInit {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;

    constructor() {}

    ngOnInit(): void {}
}
