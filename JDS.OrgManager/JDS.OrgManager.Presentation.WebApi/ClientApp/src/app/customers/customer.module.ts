// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";
import { CoreModule } from "../core/core.module";

import { AddOrUpdateCustomerComponent } from "./add-or-update-customer/add-or-update-customer.component";
import { ManageTenantsComponent } from "./manage-tenants/manage-tenants.component";
import { CustomerClient, TenantClient, UserClient } from "../shared/nswag";
import { CustomerHomeComponent } from "./customer-home/customer-home.component";

@NgModule({
    declarations: [AddOrUpdateCustomerComponent, ManageTenantsComponent, CustomerHomeComponent],
    imports: [CommonModule, SharedModule, CoreModule],
    exports: [],
    providers: [UserClient, TenantClient, CustomerClient]
})
export class CustomerModule {}
