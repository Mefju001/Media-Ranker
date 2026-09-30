import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDetailsForm } from './edit-details-form';

describe('EditDetailsForm', () => {
  let component: EditDetailsForm;
  let fixture: ComponentFixture<EditDetailsForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditDetailsForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditDetailsForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
