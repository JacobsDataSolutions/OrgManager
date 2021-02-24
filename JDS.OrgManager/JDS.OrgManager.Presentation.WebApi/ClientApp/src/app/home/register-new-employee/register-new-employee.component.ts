import { Component, OnInit, ChangeDetectionStrategy, OnDestroy } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Validators, FormBuilder } from "@angular/forms";
import { Observable, Subject } from "rxjs";
import { EmployeeClient, EmployeeViewModel } from "../../shared/nswag";
import { GuidValidator } from "../../shared/utils";
import { States } from "../../shared/consts";

import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";
import { isValidGuidValidator } from "../../shared/validator-functions";
import { takeUntil } from "rxjs/operators";

@Component({
    selector: "om-register-new-employee",
    templateUrl: "./register-new-employee.component.html",
    styleUrls: ["./register-new-employee.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class RegisterNewEmployeeComponent implements OnInit, OnDestroy {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    private ngUnsubscribe = new Subject();
    states = States;
    hasValidAssignmentKey;
    form = this.fb.group({
        assignmentKey: ["", [Validators.required, Validators.maxLength(36), Validators.minLength(36), isValidGuidValidator()]],
        firstName: ["", [Validators.required, Validators.maxLength(25)]],
        middleName: ["", [Validators.maxLength(25)]],
        lastName: ["", [Validators.required, Validators.maxLength(35)]],
        gender: [undefined, [Validators.required]],
        dateOfBirth: ["", [Validators.required]],
        address1: ["", [Validators.required, Validators.maxLength(50)]],
        address2: ["", [Validators.maxLength(15)]],
        city: ["", [Validators.required, Validators.maxLength(30)]],
        state: ["", [Validators.required]],
        zip: ["", [Validators.required, Validators.maxLength(10)]],
        externalEmployeeId: ["", [Validators.maxLength(25)]]
    });

    formValueChanges$: Observable<EmployeeViewModel>;

    constructor(private route: ActivatedRoute, private fb: FormBuilder, private employeeClient: EmployeeClient, private router: Router) {}

    ngOnInit(): void {
        this.route.queryParamMap.subscribe((p) => {
            const assignmentKey = p.get("assignmentKey");
            this.hasValidAssignmentKey = GuidValidator.isValidGuid(assignmentKey);
            if (assignmentKey) {
                this.form.patchValue({ assignmentKey: assignmentKey });
            }
        });
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    save() {
        if (this.form.valid) {
            const employee = new EmployeeViewModel();
            employee.init(this.form.value);
            this.employeeClient
                .registerNewEmployee(employee)
                .pipe(takeUntil(this.ngUnsubscribe))
                .subscribe(() => {
                    this.router.navigate(["/employee-registered"]);
                });
        }
    }
}
