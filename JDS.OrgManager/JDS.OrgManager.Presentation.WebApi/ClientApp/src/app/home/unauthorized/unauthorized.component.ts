// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { Location } from "@angular/common";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";

@Component({
    selector: "om-unauthorized",
    templateUrl: "./unauthorized.component.html",
    styleUrls: ["./unauthorized.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class UnauthorizedComponent implements OnInit {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    constructor(private location: Location) {}

    ngOnInit(): void {}

    goBack() {
        this.location.back();
        this.location.back();
    }
}
