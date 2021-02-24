import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmitTimeOffRequestComponent } from './submit-time-off-request.component';

describe('SubmitTimeOffRequestComponent', () => {
  let component: SubmitTimeOffRequestComponent;
  let fixture: ComponentFixture<SubmitTimeOffRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SubmitTimeOffRequestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmitTimeOffRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
