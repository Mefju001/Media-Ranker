import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditGameForm } from './edit-game-form';

describe('EditGameForm', () => {
  let component: EditGameForm;
  let fixture: ComponentFixture<EditGameForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditGameForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditGameForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
