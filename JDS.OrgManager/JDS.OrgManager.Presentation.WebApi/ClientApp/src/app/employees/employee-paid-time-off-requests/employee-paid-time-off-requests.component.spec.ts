import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeePaidTimeOffRequestsComponent } from './employee-paid-time-off-requests.component';

describe('EmployeePaidTimeOffRequestsComponent', () => {
  let component: EmployeePaidTimeOffRequestsComponent;
  let fixture: ComponentFixture<EmployeePaidTimeOffRequestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeePaidTimeOffRequestsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePaidTimeOffRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
