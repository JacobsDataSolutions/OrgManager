// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";

@Component({
    selector: "org-manager-employee-home",
    templateUrl: "./employee-home.component.html",
    styleUrls: ["./employee-home.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class EmployeeHomeComponent implements OnInit {
    constructor() {}

    ngOnInit(): void {}
}
