// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import browser from "browser-detect";
import { Component, OnInit } from "@angular/core";

import { environment as env } from "../../environments/environment";

import { routeAnimations, LocalStorageService } from "../core/core.module";
import { AuthorizeService } from "../../api-authorization/authorize.service";
import { OverlayContainer } from "@angular/cdk/overlay";

@Component({
    selector: "om-root",
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
    navigation = [
        //{ link: "about", label: "About" },
        //{ link: "feature-list", label: "Features" },
        //{ link: "examples", label: "Examples" }
    ];
    navigationSideMenu = [{ link: "settings", label: "Settings" }];

    isAuthenticated = false;
    stickyHeader = true;
    theme = "default-theme";

    constructor(private authorizeService: AuthorizeService, private storageService: LocalStorageService, private overlayContainer: OverlayContainer) {}

    private static isIEorEdgeOrSafari() {
        return ["ie", "edge", "safari"].includes(browser().name);
    }

    ngOnInit(): void {
        this.storageService.testLocalStorage();
        if (AppComponent.isIEorEdgeOrSafari()) {
            //TODO: disable animations.
        }

        this.authorizeService.isAuthenticated().subscribe((r) => {
            this.isAuthenticated = r;
        });

        // Set theme.
        const classList = this.overlayContainer.getContainerElement().classList;
        const toRemove = Array.from(classList).filter((item: string) => item.includes("-theme"));
        if (toRemove.length) {
            classList.remove(...toRemove);
        }
        classList.add("default-theme");
    }
}
