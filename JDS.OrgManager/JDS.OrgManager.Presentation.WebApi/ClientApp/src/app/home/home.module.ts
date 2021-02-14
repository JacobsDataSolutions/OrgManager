import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { SharedModule } from "../shared/shared.module";

import { HomeComponent } from "./home.component";

import { LetsGetStartedComponent } from "./lets-get-started/lets-get-started.component";
import { RegisterNewEmployeeComponent } from "./register-new-employee/register-new-employee.component";

import { RouterModule } from "@angular/router";
import { EmployeeRegisteredComponent } from "./employee-registered/employee-registered.component";
import { CoreModule } from "../core/core.module";
import { UnauthorizedComponent } from "./unauthorized/unauthorized.component";
import { UserClient } from "../shared/nswag";

@NgModule({
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    declarations: [HomeComponent, LetsGetStartedComponent, RegisterNewEmployeeComponent, EmployeeRegisteredComponent, UnauthorizedComponent],
    imports: [RouterModule, CoreModule, CommonModule, SharedModule],
    exports: [],
    providers: [UserClient]
})
export class HomeModule {}
