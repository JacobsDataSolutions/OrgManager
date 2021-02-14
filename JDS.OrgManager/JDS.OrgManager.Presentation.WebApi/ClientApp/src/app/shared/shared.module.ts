import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { BigInputComponent } from "./big-input/big-input/big-input.component";
import { BigInputActionComponent } from "./big-input/big-input-action/big-input-action.component";
import { CoreModule } from "../core/core.module";

@NgModule({
    imports: [CommonModule, CoreModule],
    declarations: [BigInputComponent, BigInputActionComponent],
    exports: [CommonModule, BigInputComponent, BigInputActionComponent]
})
export class SharedModule {
    constructor() {}
}
