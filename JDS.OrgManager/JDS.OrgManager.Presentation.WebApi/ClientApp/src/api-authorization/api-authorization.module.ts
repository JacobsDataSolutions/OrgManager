// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LoginMenuComponent } from "./login-menu/login-menu.component";
import { LoginComponent } from "./login/login.component";
import { LogoutComponent } from "./logout/logout.component";
import { RouterModule } from "@angular/router";
import { ApplicationPaths } from "./api-authorization.constants";
import { HttpClientModule } from "@angular/common/http";

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        RouterModule.forChild([
            { path: ApplicationPaths.Register, component: LoginComponent },
            { path: ApplicationPaths.Profile, component: LoginComponent },
            { path: ApplicationPaths.Login, component: LoginComponent },
            { path: ApplicationPaths.LoginFailed, component: LoginComponent },
            { path: ApplicationPaths.LoginCallback, component: LoginComponent },
            { path: ApplicationPaths.LogOut, component: LogoutComponent },
            { path: ApplicationPaths.LoggedOut, component: LogoutComponent },
            { path: ApplicationPaths.LogOutCallback, component: LogoutComponent }
        ])
    ],
    declarations: [LoginMenuComponent, LoginComponent, LogoutComponent],
    exports: [LoginMenuComponent, LoginComponent, LogoutComponent]
})
export class ApiAuthorizationModule {}
