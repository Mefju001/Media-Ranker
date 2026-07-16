import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditTvSeriesForm } from './edit-tv-series-form';

describe('EditTvSeriesForm', () => {
  let component: EditTvSeriesForm;
  let fixture: ComponentFixture<EditTvSeriesForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditTvSeriesForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditTvSeriesForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
