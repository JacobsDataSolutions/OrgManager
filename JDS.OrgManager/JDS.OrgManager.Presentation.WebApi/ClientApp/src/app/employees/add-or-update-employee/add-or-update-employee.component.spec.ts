import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { AddOrUpdateEmployeeComponent } from "./add-or-update-employee.component";

describe("AddOrUpdateEmployeeComponent", () => {
    let component: AddOrUpdateEmployeeComponent;
    let fixture: ComponentFixture<AddOrUpdateEmployeeComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AddOrUpdateEmployeeComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(AddOrUpdateEmployeeComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it("should create", () => {
        expect(component).toBeTruthy();
    });
});
