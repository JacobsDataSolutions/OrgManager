import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { Router } from "@angular/router";
import { Location } from "@angular/common";

@Component({
    selector: "org-manager-unauthorized",
    templateUrl: "./unauthorized.component.html",
    styleUrls: ["./unauthorized.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UnauthorizedComponent implements OnInit {
    image = require("../../../assets/unauthorized.jpg").default;
    constructor(private router: Router, private location: Location) {}

    ngOnInit(): void {}

    goBack() {
        this.location.back();
        this.location.back();
    }
}
