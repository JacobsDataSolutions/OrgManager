// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy, OnDestroy } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { Router } from "@angular/router";

import { ROUTE_ANIMATIONS_ELEMENTS } from "../core/core.module";

// JDS
import { AuthorizeService } from "../../api-authorization/authorize.service";
import { UserClient } from "../shared/nswag";
import { takeUntil } from "rxjs/operators";

@Component({
    selector: "om-home",
    templateUrl: "./home.component.html",
    styleUrls: ["./home.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class HomeComponent implements OnInit, OnDestroy {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    private ngUnsubscribe = new Subject();

    isAuthenticated$: Observable<boolean>;
    constructor(private authorizeService: AuthorizeService, private userClient: UserClient, private router: Router) {}

    ngOnInit(): void {
        this.authorizeService
            .isAuthenticated()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((auth) => {
                if (auth) {
                    this.userClient
                        .getUserStatus(null)
                        .pipe(takeUntil(this.ngUnsubscribe))
                        .subscribe((userStatus) => {
                            if (userStatus.isCustomer) {
                                if (userStatus.hasProvidedCustomerInformation) {
                                    console.log("Redirect to customer dashboard.");
                                } else {
                                    console.log("Redirect to customer information page.");
                                }
                            } else {
                                if (userStatus.hasProvidedEmployeeInformation) {
                                    //this.router.navigate
                                } else {
                                    console.log("Redirect to register new employee page");
                                }
                            }
                        });
                }
            });
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    onSignUpClick() {
        this.router.navigate(["/lets-get-started"]);
    }
}
