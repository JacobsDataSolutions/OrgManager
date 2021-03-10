// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";

import { HomeComponent } from "./home.component";

import { LetsGetStartedComponent } from "./lets-get-started/lets-get-started.component";

import { RouterModule } from "@angular/router";
import { EmployeeRegisteredComponent } from "./employee-registered/employee-registered.component";
import { CoreModule } from "../core/core.module";
import { UnauthorizedComponent } from "./unauthorized/unauthorized.component";
import { UserClient } from "../shared/nswag";

@NgModule({
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    declarations: [HomeComponent, LetsGetStartedComponent, EmployeeRegisteredComponent, UnauthorizedComponent],
    imports: [RouterModule, CoreModule, CommonModule, SharedModule],
    exports: [],
    providers: [UserClient]
})
export class HomeModule {}
