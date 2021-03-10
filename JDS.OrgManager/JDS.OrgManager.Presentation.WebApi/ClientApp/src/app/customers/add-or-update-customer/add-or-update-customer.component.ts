// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy, OnDestroy } from "@angular/core";
import { Validators, FormBuilder } from "@angular/forms";
import { Subject } from "rxjs";

import { CustomerViewModel, CustomerClient } from "../../shared/nswag";
import { States, Currencies } from "../../shared/consts";

import { ROUTE_ANIMATIONS_ELEMENTS, NotificationService } from "../../core/core.module";
import { takeUntil } from "rxjs/operators";
import { Lengths } from "../../shared/lengths";

@Component({
    selector: "om-add-or-update-customer",
    templateUrl: "./add-or-update-customer.component.html",
    styleUrls: ["./add-or-update-customer.component.css"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class AddOrUpdateCustomerComponent implements OnInit, OnDestroy {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    lengths = Lengths;
    private ngUnsubscribe = new Subject();
    states = States;
    currencies = Currencies;
    form = this.fb.group({
        companyName: ["", [Validators.required, Validators.maxLength(Lengths._name)]],
        firstName: ["", [Validators.required, Validators.maxLength(Lengths.firstName)]],
        middleName: ["", [Validators.maxLength(Lengths.middleName)]],
        lastName: ["", [Validators.required, Validators.maxLength(Lengths.lastName)]],
        title: [undefined],
        address1: ["", [Validators.required, Validators.maxLength(Lengths.address1)]],
        address2: ["", [Validators.maxLength(Lengths.address2)]],
        city: ["", [Validators.required, Validators.maxLength(Lengths.city)]],
        state: ["", [Validators.required]],
        zipCode: ["", [Validators.required, Validators.maxLength(Lengths.zipCode)]],
        currencyCode: ["", [Validators.required]]
    });

    constructor(private fb: FormBuilder, private customerClient: CustomerClient, private notificationService: NotificationService) {}

    ngOnInit() {
        this.customerClient
            .getCustomer()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((customer) => {
                if (customer) {
                    this.form.patchValue(customer);
                }
            });
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    save() {
        if (this.form.valid) {
            const customer = new CustomerViewModel();
            customer.init(this.form.value);
            this.customerClient
                .addOrUpdateCustomer(customer)
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe((result) => {
                    if (result) {
                        this.form.patchValue(result);
                        this.notificationService.success("Customer details successfully updated.");
                    }
                });
        }
    }
}
