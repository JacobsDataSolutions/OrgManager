import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";

@Component({
    selector: "om-add-or-update-employee",
    templateUrl: "./add-or-update-employee.component.html",
    styleUrls: ["./add-or-update-employee.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddOrUpdateEmployeeComponent implements OnInit {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    tenantSlug: string;
    constructor(private route: ActivatedRoute) {}

    ngOnInit(): void {
        this.tenantSlug = this.route.snapshot.paramMap.get("tenantSlug");
        alert(this.tenantSlug);
    }
}
