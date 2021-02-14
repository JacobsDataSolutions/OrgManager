import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { EmployeeRegisteredComponent } from "./employee-registered.component";

describe("EmployeeRegisteredComponent", () => {
    let component: EmployeeRegisteredComponent;
    let fixture: ComponentFixture<EmployeeRegisteredComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [EmployeeRegisteredComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(EmployeeRegisteredComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it("should create", () => {
        expect(component).toBeTruthy();
    });
});
