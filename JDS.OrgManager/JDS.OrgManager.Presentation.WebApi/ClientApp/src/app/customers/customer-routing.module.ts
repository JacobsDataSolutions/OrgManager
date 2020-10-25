// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ManageTenantsComponent } from "./manage-tenants/manage-tenants.component";
import { AuthorizeGuard } from "../../api-authorization/authorize.guard";
import { HasCustomerAccessGuard } from "./has-customer-access.guard";
import { AddOrUpdateCustomerComponent } from "./add-or-update-customer/add-or-update-customer.component";

const routes: Routes = [
    {
        path: "customer",
        component: AddOrUpdateCustomerComponent,
        canActivate: [AuthorizeGuard, HasCustomerAccessGuard]
    },
    {
        path: "customer/manage-tenants",
        component: ManageTenantsComponent,
        canActivate: [AuthorizeGuard, HasCustomerAccessGuard]
    }
];

@NgModule({
    // useHash supports github.io demo page, remove in your app
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class CustomerRoutingModule {}
