import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";
import { CoreModule } from "../core/core.module";

import { AddOrUpdateEmployeeComponent } from "./add-or-update-employee/add-or-update-employee.component";
import { EmployeeHomeComponent } from "./employee-home/employee-home.component";
import { RouterModule } from "@angular/router";
import { EmployeeClient, UserClient, TimeOffClient, TenantClient } from "../shared/nswag";
import { EmployeePaidTimeOffRequestsComponent } from "./employee-paid-time-off-requests/employee-paid-time-off-requests.component";
import { MatTableModule } from "@angular/material/table";
import { MatPaginatorModule } from "@angular/material/paginator";

@NgModule({
    declarations: [AddOrUpdateEmployeeComponent, EmployeeHomeComponent, EmployeePaidTimeOffRequestsComponent],
    imports: [CommonModule, SharedModule, CoreModule, RouterModule, MatTableModule, MatPaginatorModule],
    exports: [],
    providers: [EmployeeClient, UserClient, TimeOffClient, TenantClient]
})
export class EmployeeModule {}
