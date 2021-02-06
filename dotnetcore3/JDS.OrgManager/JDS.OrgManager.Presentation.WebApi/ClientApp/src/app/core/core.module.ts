// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { CommonModule } from "@angular/common";
import { NgModule, Optional, SkipSelf, ErrorHandler } from "@angular/core";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import {
    FaIconLibrary,
    FontAwesomeModule
} from "@fortawesome/angular-fontawesome";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatListModule } from "@angular/material/list";
import { MatMenuModule } from "@angular/material/menu";
import { MatIconModule } from "@angular/material/icon";
import { MatSelectModule } from "@angular/material/select";
import { MatTooltipModule } from "@angular/material/tooltip";
import { FormsModule } from "@angular/forms";
import { MatSnackBarModule } from "@angular/material/snack-bar";

// JDS
import { ApiAuthorizationModule } from "../../api-authorization/api-authorization.module";
import { AuthorizeInterceptor } from "../../api-authorization/authorize.interceptor";

import {
    ROUTE_ANIMATIONS_ELEMENTS,
    routeAnimations
} from "./animations/route.animations";
import { AnimationsService } from "./animations/animations.service";
import { AppErrorHandler } from "./error-handler/app-error-handler.service";
import { LocalStorageService } from "./local-storage/local-storage.service";
import { HttpErrorInterceptor } from "./http-interceptors/http-error.interceptor";
import { NotificationService } from "./notifications/notification.service";
import { MatButtonModule } from "@angular/material/button";
import {
    faCog,
    faBars,
    faRocket,
    faPowerOff,
    faUserCircle,
    faPlayCircle
} from "@fortawesome/free-solid-svg-icons";
import {
    faGithub,
    faMediumM,
    faTwitter,
    faInstagram,
    faYoutube
} from "@fortawesome/free-brands-svg-icons";

export {
    routeAnimations,
    LocalStorageService,
    ROUTE_ANIMATIONS_ELEMENTS,
    AnimationsService,
    NotificationService
};

@NgModule({
    imports: [
        // JDS
        ApiAuthorizationModule,

        // angular
        CommonModule,
        HttpClientModule,
        FormsModule,

        // material
        MatSidenavModule,
        MatToolbarModule,
        MatListModule,
        MatMenuModule,
        MatIconModule,
        MatSelectModule,
        MatTooltipModule,
        MatSnackBarModule,
        MatButtonModule,

        // 3rd party
        FontAwesomeModule
    ],
    declarations: [],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: HttpErrorInterceptor,
            multi: true
        },
        { provide: ErrorHandler, useClass: AppErrorHandler },
        // JDS
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthorizeInterceptor,
            multi: true
        }
    ],
    exports: [
        // angular
        FormsModule,

        // material
        MatSidenavModule,
        MatToolbarModule,
        MatListModule,
        MatMenuModule,
        MatIconModule,
        MatSelectModule,
        MatTooltipModule,
        MatSnackBarModule,
        MatButtonModule,

        // 3rd party
        FontAwesomeModule
    ]
})
export class CoreModule {
    constructor(
        @Optional()
        @SkipSelf()
        parentModule: CoreModule,
        faIconLibrary: FaIconLibrary
    ) {
        if (parentModule) {
            throw new Error(
                "CoreModule is already loaded. Import only in AppModule"
            );
        }
        faIconLibrary.addIcons(
            faCog,
            faBars,
            faRocket,
            faPowerOff,
            faUserCircle,
            faPlayCircle,
            faGithub,
            faMediumM,
            faTwitter,
            faInstagram,
            faYoutube
        );
    }
}
