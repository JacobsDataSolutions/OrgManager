// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthorizeGuard } from "../../api-authorization/authorize.guard";
import { EmployeeHomeComponent } from "./employee-home/employee-home.component";
import { HasTenantAccessGuard } from "../tenants/has-tenant-access.guard";

const routes: Routes = [
    {
        path: "t/:slug",
        component: EmployeeHomeComponent,
        canActivate: [AuthorizeGuard, HasTenantAccessGuard]
    },
    {
        path: "t/:slug/employee-home",
        component: EmployeeHomeComponent,
        canActivate: [AuthorizeGuard, HasTenantAccessGuard]
    }
];

@NgModule({
    // useHash supports github.io demo page, remove in your app
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class EmployeeRoutingModule {}
