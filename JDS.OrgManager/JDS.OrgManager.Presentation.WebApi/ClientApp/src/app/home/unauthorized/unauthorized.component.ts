import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { Location } from "@angular/common";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";

@Component({
    selector: "om-unauthorized",
    templateUrl: "./unauthorized.component.html",
    styleUrls: ["./unauthorized.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UnauthorizedComponent implements OnInit {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    constructor(private location: Location) {}

    ngOnInit(): void {}

    goBack() {
        this.location.back();
        this.location.back();
    }
}
