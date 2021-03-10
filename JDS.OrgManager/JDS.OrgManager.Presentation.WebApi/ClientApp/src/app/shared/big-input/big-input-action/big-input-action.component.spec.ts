// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { NoopAnimationsModule } from "@angular/platform-browser/animations";
import { By } from "@angular/platform-browser";
import { Component } from "@angular/core";
import { ComponentFixture, TestBed } from "@angular/core/testing";

import { SharedModule } from "../../shared.module";

@Component({
    selector: "om-host-for-test",
    template: ""
})
class HostComponent {
    actionHandler = () => {};
}

describe("BigInputActionComponent", () => {
    let component: HostComponent;
    let fixture: ComponentFixture<HostComponent>;

    const getButton = () => fixture.debugElement.query(By.css("button"));
    const getIcon = () => fixture.debugElement.query(By.css("mat-icon"));
    const getLabel = () => fixture.debugElement.query(By.css(".mat-button-wrapper > span"));

    function createHostComponent(template: string): ComponentFixture<HostComponent> {
        TestBed.overrideComponent(HostComponent, { set: { template: template } });
        fixture = TestBed.createComponent(HostComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
        return fixture;
    }

    beforeEach(() =>
        TestBed.configureTestingModule({
            declarations: [HostComponent],
            imports: [SharedModule, NoopAnimationsModule]
        })
    );

    it("should be created", () => {
        const template = "<om-big-input-action></om-big-input-action>";
        fixture = createHostComponent(template);
        expect(component).toBeTruthy();
    });

    it("should initially not be disabled and show no icon or label", () => {
        const template = "<om-big-input-action></om-big-input-action>";
        fixture = createHostComponent(template);
        expect(getButton().nativeElement.disabled).toBeFalsy();
        expect(getIcon()).toBeNull();
        expect(getLabel()).toBeNull();
    });

    it("should disable button if disabled property is set", () => {
        const template = '<om-big-input-action [disabled]="true"></om-big-input-action>';
        fixture = createHostComponent(template);
        expect(getButton().nativeElement.disabled).toBeTruthy();
    });

    it("should display icon if fontSet and fontIcon properties are set", () => {
        const template = '<om-big-input-action fontSet="fas" fontIcon="fa-trash"></om-big-input-action>';
        fixture = createHostComponent(template);
        expect(getIcon()).toBeTruthy();
        expect(getIcon().nativeElement.classList.contains("fa-trash")).toBeTruthy();
        expect(getIcon().nativeElement.classList.contains("fas")).toBeTruthy();
    });

    it("should display label with provided text when label property is set", () => {
        const template = '<om-big-input-action label="delete"></om-big-input-action>';
        fixture = createHostComponent(template);
        expect(getLabel()).toBeTruthy();
        expect(getLabel().nativeElement.textContent).toBe("delete");
    });

    it("should emit action event on button click", () => {
        const template = '<om-big-input-action (action)="actionHandler()"></om-big-input-action>';
        fixture = createHostComponent(template);
        spyOn(component, "actionHandler").and.callThrough();
        getButton().triggerEventHandler("click", {});
        expect(component.actionHandler).toHaveBeenCalled();
    });
});
