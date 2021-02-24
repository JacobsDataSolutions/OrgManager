import { Component, OnInit, ChangeDetectionStrategy, Input } from "@angular/core";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";
import { PaidTimeOffRequestStatus, PaidTimeOffRequestViewModel } from "../../shared/nswag";

@Component({
    selector: "om-employee-paid-time-off-requests",
    templateUrl: "./employee-paid-time-off-requests.component.html",
    styleUrls: ["./employee-paid-time-off-requests.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class EmployeePaidTimeOffRequestsComponent implements OnInit {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    @Input() paidTimeOffRequests: PaidTimeOffRequestViewModel[] = [];

    displayedColumns = ["startDate", "endDate", "hoursRequested", "submittedByName", "status", "notes"];

    constructor() {}

    ngOnInit(): void {}

    getStatus(status: PaidTimeOffRequestStatus): string {
        return Object.keys(PaidTimeOffRequestStatus).find((key) => PaidTimeOffRequestStatus[key] === status);
    }
}
