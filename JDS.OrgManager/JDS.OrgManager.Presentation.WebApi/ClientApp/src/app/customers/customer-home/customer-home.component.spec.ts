import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerHomeComponent } from './customer-home.component';

describe('CustomerHomeComponent', () => {
  let component: CustomerHomeComponent;
  let fixture: ComponentFixture<CustomerHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerHomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
