import { Component, OnInit, ChangeDetectionStrategy, OnDestroy } from "@angular/core";
import { EmployeeClient, UserClient, UserStatusViewModel } from "../../shared/nswag";
import { ROUTE_ANIMATIONS_ELEMENTS } from "../../core/core.module";
import { ActivatedRoute, Router } from "@angular/router";
import { TenantService } from "../../tenants/tenant.service";
import { Subject } from "rxjs";
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
    numPendingEmployees = 0;
    tenantId = 0;
    constructor(
        private tenantService: TenantService,
        private router: Router,
        private userClient: UserClient,
        private employeeClient: EmployeeClient,
        private route: ActivatedRoute
    ) {}

    ngOnInit() {
        this.tenantService
            .getCachedTenantIdFromSlug(this.route.snapshot.paramMap.get("slug"))
            .pipe(takeUntil(this.ngUnsubscribe))
            .subscribe((tenantId) => {
                this.tenantId = tenantId;
                // TODO: reimplement user service in order to cache the user status?
                this.userClient
                    .getUserStatus(tenantId)
                    .pipe(takeUntil(this.ngUnsubscribe))
                    .subscribe((userStatus) => {
                        this.userStatus = userStatus;
                        if (userStatus.isCustomer) {
                        }
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
