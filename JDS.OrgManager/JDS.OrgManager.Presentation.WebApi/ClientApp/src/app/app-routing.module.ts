// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UnauthorizedComponent } from "./home/unauthorized/unauthorized.component";
import { TestComponent } from "./home/test/test.component";
import { HomeComponent } from "./home/home.component";
import { LetsGetStartedComponent } from "./home/lets-get-started/lets-get-started.component";
import { EmployeeRegisteredComponent } from "./home/employee-registered/employee-registered.component";
import { AuthorizeGuard } from "../api-authorization/authorize.guard";
import { AddOrUpdateCustomerComponent } from "./customers/add-or-update-customer/add-or-update-customer.component";
import { ManageTenantsComponent } from "./customers/manage-tenants/manage-tenants.component";
import { CustomerHomeComponent } from "./customers/customer-home/customer-home.component";
import { EmployeeHomeComponent } from "./employees/employee-home/employee-home.component";
import { AddOrUpdateEmployeeComponent } from "./employees/add-or-update-employee/add-or-update-employee.component";
import { SubmitTimeOffRequestComponent } from "./time-off/submit-time-off-request/submit-time-off-request.component";

const routes: Routes = [
    {
        path: "",
        component: HomeComponent,
        pathMatch: "full"
    },
    {
        path: "lets-get-started",
        component: LetsGetStartedComponent
    },
    {
        path: "employee-registered",
        component: EmployeeRegisteredComponent
    },
    {
        path: "unauthorized",
        component: UnauthorizedComponent
    },
    {
        path: "customer",
        component: CustomerHomeComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "customer/update",
        component: AddOrUpdateCustomerComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "customer/manage-tenants",
        component: ManageTenantsComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "test",
        component: TestComponent
    },
    {
        path: "t/:tenantSlug",
        component: EmployeeHomeComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "t/:tenantSlug/employee",
        component: EmployeeHomeComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "t/:tenantSlug/employee/update",
        component: AddOrUpdateEmployeeComponent,
        canActivate: [AuthorizeGuard]
    }
];

@NgModule({
    // useHash supports github.io demo page, remove in your app
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}
