import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";
import { CoreModule } from "../core/core.module";

import { AddOrUpdateEmployeeComponent } from "./add-or-update-employee/add-or-update-employee.component";
import { EmployeeHomeComponent } from "./employee-home/employee-home.component";
import { RouterModule } from "@angular/router";
import { EmployeeClient, UserClient, TimeOffClient, TenantClient } from "../shared/nswag";

@NgModule({
    declarations: [AddOrUpdateEmployeeComponent, EmployeeHomeComponent],
    imports: [CommonModule, SharedModule, CoreModule, RouterModule],
    exports: [],
    providers: [EmployeeClient, UserClient, TimeOffClient, TenantClient]
})
export class EmployeeModule {}
