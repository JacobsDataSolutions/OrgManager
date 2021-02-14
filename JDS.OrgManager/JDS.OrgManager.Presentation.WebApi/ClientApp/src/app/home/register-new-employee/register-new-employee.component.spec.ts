import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { RegisterNewEmployeeComponent } from "./register-new-employee.component";

describe("RegisterNewEmployeeComponent", () => {
    let component: RegisterNewEmployeeComponent;
    let fixture: ComponentFixture<RegisterNewEmployeeComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [RegisterNewEmployeeComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(RegisterNewEmployeeComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it("should create", () => {
        expect(component).toBeTruthy();
    });
});
