// Copyright (c)2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import {
    FormBuilder,
    Validators,
    FormGroup,
    FormGroupDirective,
    ValidatorFn,
    AbstractControl
} from "@angular/forms";
import {
    Component,
    ChangeDetectionStrategy,
    OnInit,
    OnDestroy
} from "@angular/core";

import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";
import {
    isValidGuidValidator,
    matchesPropertyValidator
} from "../../shared/validator-functions";
import {
    TenantViewModel,
    DeleteTenantViewModel,
    CustomerClient
} from "../../shared/nswag";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";
import { Lengths } from "../../shared/lengths";

@Component({
    selector: "org-manager-manage-tenants",
    templateUrl: "./manage-tenants.component.html",
    styleUrls: ["./manage-tenants.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class ManageTenantsComponent implements OnInit, OnDestroy {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    private ngUnsubscribe = new Subject();
    get lengths() {
        return Lengths;
    }

    form = this.fb.group({
        id: [undefined, []],
        name: ["", [Validators.required, Validators.maxLength(Lengths._name)]],
        slug: [
            "",
            [
                Validators.required,
                Validators.maxLength(Lengths.slug),
                Validators.minLength(Lengths.min)
            ]
        ],
        assignmentKey: [
            "",
            [
                Validators.required,
                Validators.maxLength(Lengths.guid),
                Validators.minLength(Lengths.guid),
                isValidGuidValidator()
            ]
        ]
    });
    deleteForm = this.fb.group({
        confirmationCode: [
            "",
            [
                Validators.required,
                matchesPropertyValidator(() => {
                    if (this.selectedTenant) {
                        return this.selectedTenant.name;
                    }
                    return "";
                })
            ]
        ]
    });

    tenants: TenantViewModel[] = [];
    selectedTenant: TenantViewModel;

    isEditing: boolean;
    isDeleting: boolean;

    static createTenant(): TenantViewModel {
        const tenant = {
            id: null,
            name: "",
            slug: "",
            assignmentKey: ""
        } as TenantViewModel;

        return tenant;
    }

    constructor(
        public fb: FormBuilder,
        private customerClient: CustomerClient
    ) {}

    ngOnInit() {
        this.customerClient
            .getTenantsForCustomer()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((tenants) => (this.tenants = tenants));
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    select(tenant: TenantViewModel) {
        this.isEditing = false;
        this.selectedTenant = tenant;
    }

    deselect() {
        this.isEditing = false;
        this.selectedTenant = null;
    }

    edit(tenant: TenantViewModel) {
        this.isEditing = true;
        this.form.setValue(tenant);
    }

    addNew(formDirective: FormGroupDirective) {
        this.customerClient
            .getNewAssignmentKey()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((key) => {
                const newTenant = ManageTenantsComponent.createTenant();
                this.form.setValue(newTenant);
                formDirective.resetForm();
                this.form.reset();
                this.form.controls["assignmentKey"].patchValue(key);
                this.isEditing = true;
            });
    }

    cancelEditing() {
        this.isEditing = false;
    }

    delete(deleteFormDirective: FormGroupDirective) {
        deleteFormDirective.resetForm();
        this.deleteForm.reset();
        this.isDeleting = true;
    }

    cancelDelete() {
        this.isDeleting = false;
    }

    confirmDelete() {
        if (this.selectedTenant && this.deleteForm.valid) {
            const confirmationCode = this.deleteForm.get("confirmationCode")
                .value;
            this.customerClient
                .deleteTenant({
                    tenantId: this.selectedTenant.id,
                    confirmationCode: confirmationCode
                } as DeleteTenantViewModel)
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe((result) => {
                    this.deselect();
                    this.isDeleting = false;
                    this.tenants.splice(
                        this.tenants.findIndex((t) => t.id === result.tenantId),
                        1
                    );
                });
        }
    }

    save() {
        if (this.form.valid) {
            const tenant = this.form.value;
            this.customerClient
                .addOrUpdateTenant(tenant)
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe((result) => {
                    this.isEditing = false;
                    if (!tenant.id) {
                        this.tenants.push(result);
                    } else {
                        if (this.selectedTenant) {
                            Object.assign(this.selectedTenant, result);
                        }
                    }
                });
        }
    }
}
