// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { Component, OnInit, ChangeDetectionStrategy, Input, Output, OnDestroy, OnChanges, SimpleChanges, EventEmitter } from "@angular/core";
import {
    UserStatusViewModel,
    PaidTimeOffRequestViewModel,
    PaidTimeOffRequestValidationResult,
    TimeOffClient,
    ValidateRequestedPaidTimeOffHoursViewModel,
    SubmitNewPaidTimeOffRequestViewModel
    //SubmitNewTimeOffRequestViewModel,
    //TimeOffRequestValidationResult
} from "../../shared/nswag";
import { ActivatedRoute } from "@angular/router";
import { Validators, FormBuilder, AsyncValidatorFn, AbstractControl } from "@angular/forms";

import { ROUTE_ANIMATIONS_ELEMENTS, NotificationService } from "../../core/core.module";
import { TenantService } from "../../tenants/tenant.service";
import { Observable, of, Subject } from "rxjs";
import { debounceTime, map, switchMap, take, takeUntil } from "rxjs/operators";
import { isEmptyInputValue } from "../../shared/functions";

@Component({
    selector: "om-submit-time-off-request",
    templateUrl: "./submit-time-off-request.component.html",
    styleUrls: ["./submit-time-off-request.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class SubmitTimeOffRequestComponent implements OnInit, OnDestroy, OnChanges {
    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    private ngUnsubscribe = new Subject();

    @Output() ptoRequestsUpdated: EventEmitter<void> = new EventEmitter<void>();

    tenantId = 0;
    userStatus: UserStatusViewModel;
    form = this.fb.group(
        {
            startDate: [undefined, [Validators.required]],
            endDate: [undefined, [Validators.required]],
            forEmployeeId: [undefined, []],
            hoursRequested: [0, []],
            result: [PaidTimeOffRequestValidationResult.OK, []],
            tenantId: [0, []]
        },
        { asyncValidator: requestedPaidTimeOffHoursValidator(this.timeOffClient, this.ngUnsubscribe) }
    );

    constructor(
        private tenantService: TenantService,
        private fb: FormBuilder,
        private notificationService: NotificationService,
        private timeOffClient: TimeOffClient,
        private route: ActivatedRoute
    ) {}

    ngOnChanges(changes: SimpleChanges): void {}

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    ngOnInit(): void {
        this.tenantService
            .getCachedTenantIdFromSlug(this.route.snapshot.paramMap.get("tenantSlug"))
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((tenantId) => {
                this.tenantId = tenantId;
                this.form.patchValue({ tenantId: tenantId });
            });
    }

    submit() {
        const timeOffRequest = new SubmitNewPaidTimeOffRequestViewModel();
        timeOffRequest.init(this.form.value);
        this.timeOffClient.submitNewPaidTimeOffRequest(timeOffRequest).subscribe((result) => {
            const msg = getErrorMessageFromPtoRequestValidationResult(result.result);
            if (msg) {
                this.notificationService.error(msg);
            } else {
                this.notificationService.success("PTO request submitted.");
                this.ptoRequestsUpdated.emit();
            }
        });
    }
}

export function getErrorMessageFromPtoRequestValidationResult(result: PaidTimeOffRequestValidationResult): string {
    let msg = "";
    switch (result) {
        case PaidTimeOffRequestValidationResult.NotEnoughHours:
            msg = "Not enough time accrued.";
            break;
        case PaidTimeOffRequestValidationResult.OverlapsWithExisting:
            msg = "Overlaps with an existing PTO request.";
            break;
        case PaidTimeOffRequestValidationResult.InThePast:
            msg = "Cannot be in the past.";
            break;
        case PaidTimeOffRequestValidationResult.TooFarInTheFuture:
            msg = "Too far into the future.";
            break;
        case PaidTimeOffRequestValidationResult.StartDateAfterEndDate:
            msg = "Start date must be before end date.";
            break;
        case PaidTimeOffRequestValidationResult.OK:
            return null;
    }
    return msg;
}

export function requestedPaidTimeOffHoursValidator(timeOffClient: TimeOffClient, ngUnsubscribe: Subject<unknown>): AsyncValidatorFn {
    return (control: AbstractControl): Promise<{ [key: string]: any } | null> | Observable<{ [key: string]: any } | null> => {
        if (isEmptyInputValue(control.value)) {
            return of(null);
        } else {
            // control is a reference to the whole form.
            return control.valueChanges.pipe(
                debounceTime(500),
                take(1),
                switchMap((_) => {
                    const viewModel = new ValidateRequestedPaidTimeOffHoursViewModel();
                    viewModel.init(control.value);
                    if (viewModel.hoursRequested && viewModel.startDate && viewModel.endDate) {
                        return timeOffClient.validateRequestedPaidTimeOffHours(viewModel).pipe(
                            map((result: PaidTimeOffRequestValidationResult) => {
                                const msg = getErrorMessageFromPtoRequestValidationResult(result);
                                return !!msg ? { invalid: { message: msg } } : null;
                            }),
                            takeUntil(ngUnsubscribe)
                        );
                    }
                    return of(null);
                })
            );
        }
    };
}
