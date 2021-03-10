import { Component, OnInit, ChangeDetectionStrategy, OnDestroy } from "@angular/core";
import { EmployeeClient, PaidTimeOffRequestViewModel, TenantClient, TenantViewModel, TimeOffClient, UserClient, UserStatusViewModel } from "../../shared/nswag";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";
import { ActivatedRoute, Router } from "@angular/router";
import { TenantService } from "../../tenants/tenant.service";
import { forkJoin, Observable, Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

@Component({
    selector: "om-employee-home",
    templateUrl: "./employee-home.component.html",
    styleUrls: ["./employee-home.component.scss"],
    changeDetection: ChangeDetectionStrategy.Default
})
export class EmployeeHomeComponent implements OnInit, OnDestroy {
    private ngUnsubscribe = new Subject();

    routeAnimationsElements = ROUTE_ANIMATIONS_ELEMENTS;
    userStatus: UserStatusViewModel = {} as UserStatusViewModel;
    tenant: TenantViewModel = {} as TenantViewModel;
    paidTimeOffRequests$: Observable<PaidTimeOffRequestViewModel[]>;
    tenantId = 0;

    constructor(
        private tenantService: TenantService,
        private router: Router,
        private userClient: UserClient,
        private employeeClient: EmployeeClient,
        private timeOffClient: TimeOffClient,
        private tenantClient: TenantClient,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        this.tenantService
            .getCachedTenantIdFromSlug(this.route.snapshot.paramMap.get("tenantSlug"))
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((tenantId) => {
                this.tenantId = tenantId;
                forkJoin(this.userClient.getUserStatus(tenantId), this.tenantClient.getTenant(tenantId)).subscribe(([userStatus, tenant]) => {
                    this.userStatus = userStatus;
                    this.tenant = tenant;
                    this.paidTimeOffRequests$ = this.timeOffClient.getPaidTimeOffRequestsForEmployee(null, tenantId);
                });
            });
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    manageEmployeesLinkClick() {
        this.router.navigate([this.tenantService.getTenantLink("manage-employees")]);
    }
}
