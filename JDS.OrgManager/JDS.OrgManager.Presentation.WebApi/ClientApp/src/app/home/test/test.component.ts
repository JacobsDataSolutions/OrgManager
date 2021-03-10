// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy, OnDestroy } from "@angular/core";
import { TestClient } from "../../shared/nswag";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";

@Component({
    selector: "om-test",
    templateUrl: "./test.component.html",
    styleUrls: ["./test.component.scss"],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TestComponent implements OnInit, OnDestroy {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    private ngUnsubscribe = new Subject();

    constructor(private testClient: TestClient) {}

    ngOnInit(): void {}

    onTestApplicationLayerException() {
        this.testClient.testApplicationLayerException().pipe(takeUntil(this.ngUnsubscribe)).subscribe();
    }

    onTestAuthorizationException() {
        this.testClient.testAuthorizationException().pipe(takeUntil(this.ngUnsubscribe)).subscribe();
    }

    onTestNotFoundException() {
        this.testClient.testNotFoundException().pipe(takeUntil(this.ngUnsubscribe)).subscribe();
    }

    onTestInvalidOperationException() {
        this.testClient.testInvalidOperationException().pipe(takeUntil(this.ngUnsubscribe)).subscribe();
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }
}
