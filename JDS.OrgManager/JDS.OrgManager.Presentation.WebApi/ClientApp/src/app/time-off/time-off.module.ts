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
    exports: [],
    providers: [TimeOffClient]
})
export class TimeOffModule {}
