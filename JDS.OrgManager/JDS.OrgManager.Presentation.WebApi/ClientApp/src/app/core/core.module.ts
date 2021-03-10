// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { CommonModule } from "@angular/common";
import { ErrorHandler, NgModule, Optional, SkipSelf } from "@angular/core";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { FaIconLibrary, FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatSidenavModule } from "@angular/material/sidenav";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatListModule } from "@angular/material/list";
import { MatMenuModule } from "@angular/material/menu";
import { MatIconModule } from "@angular/material/icon";
import { MatSelectModule } from "@angular/material/select";
import { MatTooltipModule } from "@angular/material/tooltip";
import { MatTableModule } from "@angular/material/table";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatButtonModule } from "@angular/material/button";
import { MatTabsModule } from "@angular/material/tabs";
import { MatInputModule } from "@angular/material/input";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { MatChipsModule } from "@angular/material/chips";
import { MatCheckboxModule } from "@angular/material/checkbox";
import { MatCardModule } from "@angular/material/card";
import { MatSlideToggleModule } from "@angular/material/slide-toggle";
import { MatDividerModule } from "@angular/material/divider";
import { MatSliderModule } from "@angular/material/slider";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatNativeDateModule } from "@angular/material/core";

// JDS
import { ApiAuthorizationModule } from "../../api-authorization/api-authorization.module";
import { AuthorizeInterceptor } from "../../api-authorization/authorize.interceptor";

import { ROUTE_ANIMATIONS_ELEMENTS, routeAnimations } from "./animations/route.animations";
import { AnimationsService } from "./animations/animations.service";
import { AppErrorHandler } from "./error-handler/app-error-handler.service";
import { LocalStorageService } from "./local-storage/local-storage.service";
import { HttpErrorInterceptor } from "./http-interceptors/http-error.interceptor";
import { NotificationService } from "./notifications/notification.service";

import {
    faCog,
    faBars,
    faRocket,
    faPowerOff,
    faUserCircle,
    faPlayCircle,
    faArrowRight,
    faPlus,
    faUmbrellaBeach,
    faBlog,
    faHome,
    faRedo,
    faHotel,
    faEdit,
    faTrash,
    faTimes,
    faCaretUp,
    faCaretDown,
    faExclamationTriangle,
    faFilter,
    faTasks,
    faCheck,
    faSquare,
    faLanguage,
    faPaintBrush,
    faLightbulb,
    faWindowMaximize,
    faStream,
    faBook
} from "@fortawesome/free-solid-svg-icons";
import { faGithub, faMediumM, faTwitter, faInstagram, faYoutube, faFacebook, faLinkedin } from "@fortawesome/free-brands-svg-icons";

export { routeAnimations, LocalStorageService, ROUTE_ANIMATIONS_ELEMENTS, AnimationsService, NotificationService };

@NgModule({
    imports: [
        // JDS
        ApiAuthorizationModule,

        // angular
        CommonModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,

        // material
        MatSidenavModule,
        MatToolbarModule,
        MatListModule,
        MatMenuModule,
        MatIconModule,
        MatSelectModule,
        MatTooltipModule,
        MatTableModule,
        MatSnackBarModule,
        MatButtonModule,
        MatTabsModule,
        MatInputModule,
        MatProgressSpinnerModule,
        MatChipsModule,
        MatCheckboxModule,
        MatCardModule,
        MatSlideToggleModule,
        MatDividerModule,
        MatSliderModule,
        MatDatepickerModule,
        MatNativeDateModule,

        // 3rd party
        FontAwesomeModule
    ],
    declarations: [],
    providers: [
        // JDS
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
        ReactiveFormsModule,

        // material
        MatSidenavModule,
        MatToolbarModule,
        MatListModule,
        MatMenuModule,
        MatIconModule,
        MatSelectModule,
        MatTooltipModule,
        MatTableModule,
        MatSnackBarModule,
        MatButtonModule,
        MatTabsModule,
        MatInputModule,
        MatProgressSpinnerModule,
        MatChipsModule,
        MatCheckboxModule,
        MatCardModule,
        MatSlideToggleModule,
        MatDividerModule,
        MatSliderModule,
        MatDatepickerModule,
        MatNativeDateModule,

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
            throw new Error("CoreModule is already loaded. Import only in AppModule");
        }
        faIconLibrary.addIcons(
            faCog,
            faBars,
            faRocket,
            faPowerOff,
            faUserCircle,
            faPlayCircle,
            faArrowRight,
            faPlus,
            faUmbrellaBeach,
            faBlog,
            faHome,
            faRedo,
            faHotel,
            faEdit,
            faTrash,
            faTimes,
            faCaretUp,
            faCaretDown,
            faExclamationTriangle,
            faFilter,
            faTasks,
            faCheck,
            faSquare,
            faLanguage,
            faPaintBrush,
            faLightbulb,
            faWindowMaximize,
            faStream,
            faBook,
            faGithub,
            faMediumM,
            faTwitter,
            faInstagram,
            faYoutube,
            faFacebook,
            faLinkedin
        );
    }
}
