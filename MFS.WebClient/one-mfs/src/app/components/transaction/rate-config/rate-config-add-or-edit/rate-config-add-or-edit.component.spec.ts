import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RateConfigAddOrEditComponent } from './rate-config-add-or-edit.component';

describe('RateConfigAddOrEditComponent', () => {
  let component: RateConfigAddOrEditComponent;
  let fixture: ComponentFixture<RateConfigAddOrEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RateConfigAddOrEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RateConfigAddOrEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
