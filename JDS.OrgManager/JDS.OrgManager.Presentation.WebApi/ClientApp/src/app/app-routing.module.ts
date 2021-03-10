import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { UnauthorizedComponent } from "./home/unauthorized/unauthorized.component";
import { TestComponent } from "./home/test/test.component";
import { HomeComponent } from "./home/home.component";
import { LetsGetStartedComponent } from "./home/lets-get-started/lets-get-started.component";
import { EmployeeRegisteredComponent } from "./home/employee-registered/employee-registered.component";
import { AuthorizeGuard } from "../api-authorization/authorize.guard";
import { AddOrUpdateCustomerComponent } from "./customers/add-or-update-customer/add-or-update-customer.component";
import { ManageTenantsComponent } from "./customers/manage-tenants/manage-tenants.component";
import { CustomerHomeComponent } from "./customers/customer-home/customer-home.component";
import { EmployeeHomeComponent } from "./employees/employee-home/employee-home.component";
import { AddOrUpdateEmployeeComponent } from "./employees/add-or-update-employee/add-or-update-employee.component";

const routes: Routes = [
    {
        path: "",
        component: HomeComponent,
        pathMatch: "full"
    },
    {
        path: "lets-get-started",
        component: LetsGetStartedComponent
    },
    {
        path: "employee-registered",
        component: EmployeeRegisteredComponent
    },
    {
        path: "unauthorized",
        component: UnauthorizedComponent
    },
    {
        path: "customer",
        component: CustomerHomeComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "customer/update",
        component: AddOrUpdateCustomerComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "customer/manage-tenants",
        component: ManageTenantsComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "test",
        component: TestComponent
    },
    {
        path: "t/:tenantSlug",
        component: EmployeeHomeComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "t/:tenantSlug/employee",
        component: EmployeeHomeComponent,
        canActivate: [AuthorizeGuard]
    },
    {
        path: "t/:tenantSlug/employee/update",
        component: AddOrUpdateEmployeeComponent,
        canActivate: [AuthorizeGuard]
    }
];

@NgModule({
    // useHash supports github.io demo page, remove in your app
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}
