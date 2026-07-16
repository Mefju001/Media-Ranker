import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditMovieForm } from './edit-movie-form';

describe('EditMovieForm', () => {
  let component: EditMovieForm;
  let fixture: ComponentFixture<EditMovieForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditMovieForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditMovieForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
