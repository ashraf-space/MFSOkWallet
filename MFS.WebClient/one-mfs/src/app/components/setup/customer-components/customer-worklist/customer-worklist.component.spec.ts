import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerWorklistComponent } from './customer-worklist.component';

describe('CustomerWorklistComponent', () => {
  let component: CustomerWorklistComponent;
  let fixture: ComponentFixture<CustomerWorklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerWorklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerWorklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
