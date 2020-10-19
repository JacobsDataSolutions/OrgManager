import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";
import { CoreModule } from "../core/core.module";

import { ManageTenantsComponent } from "./manage-tenants/manage-tenants.component";
import { CustomerRoutingModule } from "./customer-routing.module";
import { TenantClient, CustomerClient } from "../shared/nswag";

@NgModule({
    declarations: [ManageTenantsComponent],
    imports: [CommonModule, SharedModule, CoreModule, CustomerRoutingModule],
    exports: [],
    providers: [CustomerClient, TenantClient]
})
export class CustomerModule {}
