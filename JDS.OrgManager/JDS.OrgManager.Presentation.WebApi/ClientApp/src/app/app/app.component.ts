import browser from "browser-detect";
import { Component, OnInit } from "@angular/core";
import { Store, select } from "@ngrx/store";
import { Observable } from "rxjs";

import { environment as env } from "../../environments/environment";

import {
  authLogin,
  authLogout,
  routeAnimations,
  LocalStorageService,
  selectIsAuthenticated,
  selectSettingsStickyHeader,
  selectEffectiveTheme
} from "../core/core.module";
import { actionSettingsChangeAnimationsPageDisabled } from "../core/settings/settings.actions";

@Component({
  selector: "org-manager-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
  animations: [routeAnimations]
})
export class AppComponent implements OnInit {
  isProd = env.production;
  envName = env.envName;
  version = env.versions.app;
  year = new Date().getFullYear();
  logo = require("../../assets/logo.png").default;

  isAuthenticated$: Observable<boolean>;
  stickyHeader$: Observable<boolean>;
  theme$: Observable<string>;

  constructor(
    private store: Store,
    private storageService: LocalStorageService
  ) {}

  private static isIEorEdgeOrSafari() {
    return ["ie", "edge", "safari"].includes(browser().name);
  }

  ngOnInit(): void {
    this.storageService.testLocalStorage();
    if (AppComponent.isIEorEdgeOrSafari()) {
      this.store.dispatch(
        actionSettingsChangeAnimationsPageDisabled({
          pageAnimationsDisabled: true
        })
      );
    }

    this.isAuthenticated$ = this.store.pipe(select(selectIsAuthenticated));
    this.stickyHeader$ = this.store.pipe(select(selectSettingsStickyHeader));
    this.theme$ = this.store.pipe(select(selectEffectiveTheme));
  }

  onLoginClick() {
    this.store.dispatch(authLogin());
  }

  onLogoutClick() {
    this.store.dispatch(authLogout());
  }
}
