import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddOrUpdateCustomerComponent } from './add-or-update-customer.component';

describe('AddOrUpdateCustomerComponent', () => {
  let component: AddOrUpdateCustomerComponent;
  let fixture: ComponentFixture<AddOrUpdateCustomerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddOrUpdateCustomerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddOrUpdateCustomerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
