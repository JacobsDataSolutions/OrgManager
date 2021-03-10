// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpClient } from "@angular/common/http";

import { SharedModule } from "../shared/shared.module";
import { environment } from "../../environments/environment";
import { CoreModule } from "../core/core.module";
import { TimeOffClient } from "../shared/nswag";
import { SubmitTimeOffRequestComponent } from "./submit-time-off-request/submit-time-off-request.component";
import { EmployeePaidTimeOffRequestsComponent } from "./employee-paid-time-off-requests/employee-paid-time-off-requests.component";

@NgModule({
    declarations: [SubmitTimeOffRequestComponent, EmployeePaidTimeOffRequestsComponent],
    imports: [CommonModule, SharedModule, CoreModule],
    exports: [SubmitTimeOffRequestComponent, EmployeePaidTimeOffRequestsComponent],
    providers: [TimeOffClient]
})
export class TimeOffModule {}
