import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from "@angular/core";

import { CoreModule } from "./core/core.module";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app/app.component";
import { HomeModule } from "./home/home.module";
//import { CustomerModule } from "./customers/customer.module";
//import { EmployeeModule } from "./employees/employee.module";

@NgModule({
    imports: [
        HomeModule,
        //CustomerModule,
        //EmployeeModule,

        // angular
        BrowserAnimationsModule,
        BrowserModule,

        // core
        CoreModule,

        // app
        AppRoutingModule
    ],
    declarations: [AppComponent],
    bootstrap: [AppComponent]
})
export class AppModule {}
