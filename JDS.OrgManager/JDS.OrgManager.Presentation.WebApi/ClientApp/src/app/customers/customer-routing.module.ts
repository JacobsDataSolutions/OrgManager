import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { ManageTenantsComponent } from "./manage-tenants/manage-tenants.component";
import { AuthorizeGuard } from "../../api-authorization/authorize.guard";

const routes: Routes = [
    {
        path: "customer/manage-tenants",
        component: ManageTenantsComponent,
        canActivate: [AuthorizeGuard]
    }
];

@NgModule({
    // useHash supports github.io demo page, remove in your app
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class CustomerRoutingModule {}
