// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
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
