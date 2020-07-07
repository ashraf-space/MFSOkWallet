import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegionAddoreditComponent } from './region-addoredit.component';

describe('RegionAddoreditComponent', () => {
  let component: RegionAddoreditComponent;
  let fixture: ComponentFixture<RegionAddoreditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegionAddoreditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegionAddoreditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
