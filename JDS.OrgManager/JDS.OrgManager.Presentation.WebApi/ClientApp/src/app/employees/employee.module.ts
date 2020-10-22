// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
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
