import { CommonModule } from "@angular/common";
import { NgModule, Optional, SkipSelf, ErrorHandler } from "@angular/core";
import {
  HttpClientModule,
  HttpClient,
  HTTP_INTERCEPTORS
} from "@angular/common/http";
import {
  StoreRouterConnectingModule,
  RouterStateSerializer
} from "@ngrx/router-store";
import { StoreDevtoolsModule } from "@ngrx/store-devtools";
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

import { environment } from "../../environments/environment";

import {
  ROUTE_ANIMATIONS_ELEMENTS,
  routeAnimations
} from "./animations/route.animations";
import { AnimationsService } from "./animations/animations.service";
import { AppErrorHandler } from "./error-handler/app-error-handler.service";
import { CustomSerializer } from "./router/custom-serializer";
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

    // ngrx
    //StoreRouterConnectingModule.forRoot(),
    //environment.production
    //  ? []
    //  : StoreDevtoolsModule.instrument({
    //      name: "Org Manager"
    //    }),

    // 3rd party
    FontAwesomeModule
  ],
  declarations: [],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
    { provide: ErrorHandler, useClass: AppErrorHandler },
    { provide: RouterStateSerializer, useClass: CustomSerializer }
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
      throw new Error("CoreModule is already loaded. Import only in AppModule");
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
