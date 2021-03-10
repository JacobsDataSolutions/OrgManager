// Copyright (c)2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { AddOrUpdateCustomerComponent } from "./add-or-update-customer.component";

describe("AddOrUpdateCustomerComponent", () => {
    let component: AddOrUpdateCustomerComponent;
    let fixture: ComponentFixture<AddOrUpdateCustomerComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AddOrUpdateCustomerComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AddOrUpdateCustomerComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it("should create", () => {
        expect(component).toBeTruthy();
    });
});
