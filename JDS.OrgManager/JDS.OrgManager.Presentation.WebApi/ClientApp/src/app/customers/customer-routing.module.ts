import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ManageTenantsComponent } from "./manage-tenants/manage-tenants.component";
import { AuthorizeGuard } from "../../api-authorization/authorize.guard";
import { HasCustomerAccessGuard } from "./has-customer-access.guard";
import { AddOrUpdateCustomerComponent } from "./add-or-update-customer/add-or-update-customer.component";

const routes: Routes = [
    {
        path: "customer",
        component: AddOrUpdateCustomerComponent,
        canActivate: [AuthorizeGuard, HasCustomerAccessGuard]
    },
    {
        path: "customer/manage-tenants",
        component: ManageTenantsComponent,
        canActivate: [AuthorizeGuard, HasCustomerAccessGuard]
    }
];

@NgModule({
    // useHash supports github.io demo page, remove in your app
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class CustomerRoutingModule {}
