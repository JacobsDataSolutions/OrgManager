import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
    selector: "om-add-or-update-employee",
    templateUrl: "./add-or-update-employee.component.html",
    styleUrls: ["./add-or-update-employee.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddOrUpdateEmployeeComponent implements OnInit {
    tenantSlug: string;
    constructor(private route: ActivatedRoute) {}

    ngOnInit(): void {
        this.tenantSlug = this.route.snapshot.paramMap.get("tenantSlug");
        alert(this.tenantSlug);
    }
}
