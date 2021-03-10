// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy, Input, OnDestroy } from "@angular/core";
import {
    UserStatusViewModel,
    PaidTimeOffRequestViewModel,
    PaidTimeOffRequestValidationResult,
    TimeOffClient
    //SubmitNewTimeOffRequestViewModel,
    //TimeOffRequestValidationResult
} from "../../shared/nswag";
import { ActivatedRoute } from "@angular/router";
import { Validators, FormBuilder, AsyncValidatorFn, AbstractControl } from "@angular/forms";

import { ROUTE_ANIMATIONS_ELEMENTS, NotificationService } from "../../core/core.module";
import { TenantService } from "../../tenants/tenant.service";
import { Observable, of, Subject } from "rxjs";
import { debounceTime, switchMap, take, takeUntil } from "rxjs/operators";
import { isEmptyInputValue } from "../../shared/functions";

@Component({
    selector: "om-submit-time-off-request",
    templateUrl: "./submit-time-off-request.component.html",
    styleUrls: ["./submit-time-off-request.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class SubmitTimeOffRequestComponent implements OnInit, OnDestroy {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    private ngUnsubscribe = new Subject();

    tenantId = 0;
    userStatus: UserStatusViewModel;
    form = this.fb.group({
        timeOffTypeId: [0, [Validators.required]],
        startDate: [undefined, [Validators.required]],
        endDate: [undefined, [Validators.required]],
        forEmployeeId: [undefined],
        hoursRequested: [0],
        result: PaidTimeOffRequestValidationResult.OK,
        status: [undefined],
        tenantId: [0]
    });

    constructor(
        private tenantService: TenantService,
        private fb: FormBuilder,
        private notificationService: NotificationService,
        private timeOffClient: TimeOffClient,
        private route: ActivatedRoute
    ) {}

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    ngOnInit(): void {
        this.tenantService
            .getCachedTenantIdFromSlug(this.route.snapshot.paramMap.get("slug"))
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((tenantId) => {
                this.tenantId = tenantId;
                this.form.patchValue({ tenantId: tenantId });
            });
    }

    submit() {
        //const timeOffRequest = new SubmitNewTimeOffRequestViewModel();
        //timeOffRequest.init(this.form.value);
        //this.timeOffClient.submitNewPaidTimeOffRequest(timeOffRequest).subscribe((result) => {
        //    if (result.result === TimeOffRequestValidationResult.NotEnoughHours) {
        //        this.notificationService.error("You do not have enough hours to make the PTO request.");
        //    } else {
        //        this.notificationService.success("PTO request submitted.");
        //    }
        //});
    }

    //requestedPaidTimeOffHoursValidator(): AsyncValidatorFn {
    //    return (control: AbstractControl): Promise<{ [key: string]: any } | null> | Observable<{ [key: string]: any } | null> => {
    //        if (isEmptyInputValue(control.value)) {
    //            return of(null);
    //        } else if (this.editMode && (control.value as string).toLowerCase() === this.initialFileNamePattern.toLowerCase()) {
    //            return of(null);
    //        } else {
    //            return control.valueChanges.pipe(
    //                debounceTime(500),
    //                take(1),
    //                switchMap((_) =>
    //                    this.marketReportsClient.getFileNamePatternExists(control.value).pipe(
    //                        map((name) => (name ? { exists: { value: control.value } } : null)),
    //                        takeUntil(this.ngUnsubscribe)
    //                    )
    //                )
    //            );
    //        }
    //    };
    //}
}
