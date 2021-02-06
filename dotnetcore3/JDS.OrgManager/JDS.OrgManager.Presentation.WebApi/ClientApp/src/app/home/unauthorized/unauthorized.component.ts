// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { Router } from "@angular/router";
import { Location } from "@angular/common";

@Component({
    selector: "org-manager-unauthorized",
    templateUrl: "./unauthorized.component.html",
    styleUrls: ["./unauthorized.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UnauthorizedComponent implements OnInit {
    image = require("../../../assets/unauthorized.jpg").default;
    constructor(private router: Router, private location: Location) {}

    ngOnInit(): void {}

    goBack() {
        this.location.back();
        this.location.back();
    }
}
