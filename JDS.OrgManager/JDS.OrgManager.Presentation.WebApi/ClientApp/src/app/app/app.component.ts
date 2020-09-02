import browser from "browser-detect";
import { Component, OnInit } from "@angular/core";

import { environment as env } from "../../environments/environment";

import { routeAnimations, LocalStorageService } from "../core/core.module";

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

  isAuthenticated = false;
  stickyHeader = true;
  theme = "default-theme";

  constructor(private storageService: LocalStorageService) {}

  private static isIEorEdgeOrSafari() {
    return ["ie", "edge", "safari"].includes(browser().name);
  }

  ngOnInit(): void {
    this.storageService.testLocalStorage();
    if (AppComponent.isIEorEdgeOrSafari()) {
      //TODO: disable animations.
    }
  }

  onLoginClick() {}

  onLogoutClick() {}
}
