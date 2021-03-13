// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { forkJoin, Observable, Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { NotificationService, ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";
import { Currencies, States } from "../../shared/consts";
import { Lengths } from "../../shared/lengths";
import { AddOrUpdateEmployeeViewModel, EmployeeClient, TenantClient, TenantViewModel, UserClient, UserStatusViewModel } from "../../shared/nswag";
import { GuidValidator } from "../../shared/utils";
import { isValidGuidValidator } from "../../shared/validator-functions";
import { TenantService } from "../../tenants/tenant.service";

@Component({
    selector: "om-add-or-update-employee",
    templateUrl: "./add-or-update-employee.component.html",
    styleUrls: ["./add-or-update-employee.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class AddOrUpdateEmployeeComponent implements OnInit {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    private ngUnsubscribe = new Subject();
    lengths = Lengths;
    states = States;
    currencies = Currencies;

    tenantSlug: string;
    hasValidAssignmentKey = false;
    tenantId = 0;
    userStatus: UserStatusViewModel = {} as UserStatusViewModel;
    tenant: TenantViewModel = {} as TenantViewModel;

    form = this.fb.group({
        assignmentKey: ["", [Validators.required, Validators.maxLength(Lengths.guid), Validators.minLength(Lengths.guid), isValidGuidValidator()]],
        firstName: ["", [Validators.required, Validators.maxLength(Lengths.firstName)]],
        middleName: ["", [Validators.maxLength(Lengths.middleName)]],
        lastName: ["", [Validators.required, Validators.maxLength(Lengths.lastName)]],
        gender: [undefined, [Validators.required]],
        dateOfBirth: ["", [Validators.required]],
        address1: ["", [Validators.required, Validators.maxLength(Lengths.address1)]],
        address2: ["", [Validators.maxLength(Lengths.address2)]],
        city: ["", [Validators.required, Validators.maxLength(Lengths.city)]],
        state: ["", [Validators.required]],
        zipCode: ["", [Validators.required, Validators.maxLength(Lengths.zipCode)]],
        externalEmployeeId: ["", [Validators.maxLength(Lengths.externalEmployeeId)]],
        socialSecurityNumber: ["", [Validators.required, Validators.maxLength(Lengths.socialSecurityNumber)]],
        id: [0, []]
    });

    formValueChanges$: Observable<AddOrUpdateEmployeeViewModel>;

    constructor(
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private employeeClient: EmployeeClient,
        private router: Router,
        private notificationService: NotificationService,
        private userClient: UserClient,
        private tenantClient: TenantClient,
        private tenantService: TenantService
    ) {}

    ngOnInit(): void {
        this.route.queryParamMap.subscribe((p) => {
            let assignmentKey: string;
            this.tenantService
                .getCachedTenantIdFromSlug(this.route.snapshot.paramMap.get("tenantSlug"))
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe((tenantId) => {
                    this.tenantId = tenantId;
                    forkJoin(this.userClient.getUserStatus(tenantId), this.tenantClient.getTenant(tenantId)).subscribe(([userStatus, tenant]) => {
                        this.userStatus = userStatus;
                        this.tenant = tenant;
                        this.employeeClient
                            .getEmployee()
                            .pipe(takeUntil(this.ngUnsubscribe))
                            .subscribe((employee) => {
                                if (employee) {
                                    assignmentKey = employee.assignmentKey;
                                    this.form.patchValue(employee);
                                } else {
                                    assignmentKey = p.get("assignmentKey");
                                    if (assignmentKey) {
                                        this.form.patchValue({ assignmentKey: assignmentKey });
                                    }
                                }
                                this.hasValidAssignmentKey = GuidValidator.isValidGuid(assignmentKey);
                            });
                    });
                });
        });
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    save() {
        if (this.form.valid) {
            const employee = new AddOrUpdateEmployeeViewModel();
            employee.init(this.form.value);
            employee.tenantId = this.tenantId;
            this.employeeClient
                .addOrUpdateEmployee(employee)
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe((result) => {
                    if (result) {
                        this.form.patchValue(result);
                        this.notificationService.success("Employee details successfully updated.");
                    }
                });
        }
    }
}
