import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";
import { CoreModule } from "../core/core.module";

import { TenantClient, EmployeeClient, UserClient } from "../shared/nswag";
import { EmployeeRoutingModule } from "./employee-routing.module";

@NgModule({
    declarations: [],
    imports: [CommonModule, SharedModule, CoreModule, EmployeeRoutingModule],
    exports: [],
    providers: [EmployeeClient, TenantClient, UserClient]
})
export class EmployeeModule {}
