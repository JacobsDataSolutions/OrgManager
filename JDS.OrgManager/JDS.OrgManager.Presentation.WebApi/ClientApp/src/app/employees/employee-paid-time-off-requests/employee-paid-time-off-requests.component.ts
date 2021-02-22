import { Component, OnInit, ChangeDetectionStrategy, Input } from "@angular/core";
import { PaidTimeOffRequestViewModel } from "../../shared/nswag";

@Component({
    selector: "om-employee-paid-time-off-requests",
    templateUrl: "./employee-paid-time-off-requests.component.html",
    styleUrls: ["./employee-paid-time-off-requests.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class EmployeePaidTimeOffRequestsComponent implements OnInit {
    @Input() paidTimeOffRequests: PaidTimeOffRequestViewModel[] = [];
    displayedColumns: string[] = ["startDate", "endDate", "hoursRequested", "submittedByName", "notes"];

    constructor() {}

    ngOnInit(): void {}
}
