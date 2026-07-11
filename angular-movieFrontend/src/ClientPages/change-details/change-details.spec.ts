import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeDetails } from './change-details';

describe('ChangeDetails', () => {
  let component: ChangeDetails;
  let fixture: ComponentFixture<ChangeDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangeDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
