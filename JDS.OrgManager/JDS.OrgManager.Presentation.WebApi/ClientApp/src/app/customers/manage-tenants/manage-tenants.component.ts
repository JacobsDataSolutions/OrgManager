import { FormBuilder, Validators, FormGroupDirective } from "@angular/forms";
import { Component, ChangeDetectionStrategy, OnInit } from "@angular/core";

import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";
import { TenantViewModel, DeleteTenantViewModel, CustomerClient } from "../../shared/nswag";
import { isValidGuidValidator, matchesPropertyValidator } from "../../shared/validator-functions";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

@Component({
    selector: "om-manage-tenants",
    templateUrl: "./manage-tenants.component.html",
    styleUrls: ["./manage-tenants.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class ManageTenantsComponent implements OnInit {
    private ngUnsubscribe = new Subject();

    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;

    form = this.fb.group({
        id: [0, []],
        name: ["", [Validators.required, Validators.maxLength(50)]],
        slug: ["", [Validators.required, Validators.maxLength(25), Validators.minLength(4)]],
        assignmentKey: ["", [Validators.required, Validators.maxLength(36), Validators.minLength(36), isValidGuidValidator()]]
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
            id: 0,
            name: "",
            slug: "",
            assignmentKey: ""
        } as TenantViewModel;

        return tenant;
    }

    constructor(public fb: FormBuilder, private customerClient: CustomerClient) {}

    ngOnInit() {
        this.customerClient
            .getTenantsForCustomer()
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((t) => {
                this.tenants = t;
            });
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
                this.form.controls["id"].patchValue(0);
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
            const confirmationCode = this.deleteForm.get("confirmationCode").value;
            this.customerClient
                .deleteTenant({
                    tenantId: this.selectedTenant.id,
                    confirmationCode: confirmationCode
                } as DeleteTenantViewModel)
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
            this.customerClient.addOrUpdateTenant(tenant).subscribe((result) => {
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
