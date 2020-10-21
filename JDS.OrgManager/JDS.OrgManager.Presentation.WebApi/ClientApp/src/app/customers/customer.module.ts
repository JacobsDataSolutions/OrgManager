import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";
import { CoreModule } from "../core/core.module";

import { ManageTenantsComponent } from "./manage-tenants/manage-tenants.component";
import { CustomerRoutingModule } from "./customer-routing.module";
import { TenantClient, CustomerClient, UserClient } from "../shared/nswag";
import { AddOrUpdateCustomerComponent } from "./add-or-update-customer/add-or-update-customer.component";

@NgModule({
    declarations: [ManageTenantsComponent, AddOrUpdateCustomerComponent],
    imports: [CommonModule, SharedModule, CoreModule, CustomerRoutingModule],
    exports: [],
    providers: [CustomerClient, TenantClient, UserClient]
})
export class CustomerModule {}
