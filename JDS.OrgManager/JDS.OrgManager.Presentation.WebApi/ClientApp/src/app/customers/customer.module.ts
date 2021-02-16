import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";
import { CoreModule } from "../core/core.module";

import { AddOrUpdateCustomerComponent } from "./add-or-update-customer/add-or-update-customer.component";
import { ManageTenantsComponent } from "./manage-tenants/manage-tenants.component";
import { CustomerClient, TenantClient, UserClient } from "../shared/nswag";

@NgModule({
    declarations: [AddOrUpdateCustomerComponent, ManageTenantsComponent],
    imports: [CommonModule, SharedModule, CoreModule],
    exports: [],
    providers: [UserClient, TenantClient, CustomerClient]
})
export class CustomerModule {}
