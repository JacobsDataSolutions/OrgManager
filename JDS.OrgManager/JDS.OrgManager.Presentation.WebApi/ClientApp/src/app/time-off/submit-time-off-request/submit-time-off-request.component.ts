import { Component, OnInit, ChangeDetectionStrategy, Input, OnDestroy } from "@angular/core";
import { UserStatusViewModel, PaidTimeOffRequestViewModel, PaidTimeOffRequestValidationResult, TimeOffClient } from "../../shared/nswag";
import { ActivatedRoute } from "@angular/router";
import { Validators, FormBuilder } from "@angular/forms";

import { ROUTE_ANIMATIONS_ELEMENTS, NotificationService } from "../../core/core.module";
import { TenantService } from "../../tenants/tenant.service";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

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
        const timeOffRequest = new SubmitNewTimeOffRequestViewModel();
        timeOffRequest.init(this.form.value);
        this.timeOffClient.submitTimeOffRequest(timeOffRequest).subscribe((result) => {
            if (result.result === TimeOffRequestValidationResult.NotEnoughHours) {
                this.notificationService.error("You do not have enough hours to make the PTO request.");
            } else {
                this.notificationService.success("PTO request submitted.");
            }
        });
    }
}
