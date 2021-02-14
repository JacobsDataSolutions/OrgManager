import { async, ComponentFixture, TestBed } from "@angular/core/testing";

import { LetsGetStartedComponent } from "./lets-get-started.component";

describe("LetsGetStartedComponent", () => {
    let component: LetsGetStartedComponent;
    let fixture: ComponentFixture<LetsGetStartedComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [LetsGetStartedComponent]
        }).compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(LetsGetStartedComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it("should create", () => {
        expect(component).toBeTruthy();
    });
});
