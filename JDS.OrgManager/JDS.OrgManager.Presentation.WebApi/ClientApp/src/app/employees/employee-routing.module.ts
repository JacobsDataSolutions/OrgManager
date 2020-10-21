import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { AuthorizeGuard } from "../../api-authorization/authorize.guard";
import { EmployeeHomeComponent } from "./employee-home/employee-home.component";
import { HasTenantAccessGuard } from "../tenants/has-tenant-access.guard";

const routes: Routes = [
    {
        path: "t/:slug",
        component: EmployeeHomeComponent,
        canActivate: [AuthorizeGuard, HasTenantAccessGuard]
    },
    {
        path: "t/:slug/employee-home",
        component: EmployeeHomeComponent,
        canActivate: [AuthorizeGuard, HasTenantAccessGuard]
    }
];

@NgModule({
    // useHash supports github.io demo page, remove in your app
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class EmployeeRoutingModule {}
