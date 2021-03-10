// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
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
