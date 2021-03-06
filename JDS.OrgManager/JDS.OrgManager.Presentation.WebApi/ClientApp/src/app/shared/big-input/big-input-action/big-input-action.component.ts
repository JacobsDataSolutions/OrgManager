// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, Input, Output, EventEmitter, ChangeDetectionStrategy } from "@angular/core";

@Component({
    selector: "om-big-input-action",
    templateUrl: "./big-input-action.component.html",
    styleUrls: ["./big-input-action.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class BigInputActionComponent {
    @Input()
    disabled = false;
    @Input()
    fontSet = "";
    @Input()
    fontIcon = "";
    @Input()
    faIcon = "";
    @Input()
    label = "";
    @Input()
    color = "";

    @Output()
    action = new EventEmitter<void>();

    hasFocus = false;

    onClick() {
        this.action.emit();
    }
}
