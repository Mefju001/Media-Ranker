import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TvSeriesDetails } from './tv-series-details';

describe('TvSeriesDetails', () => {
  let component: TvSeriesDetails;
  let fixture: ComponentFixture<TvSeriesDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TvSeriesDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TvSeriesDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
