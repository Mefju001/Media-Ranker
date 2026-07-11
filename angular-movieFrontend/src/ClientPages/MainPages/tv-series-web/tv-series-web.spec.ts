import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TvSeriesWeb } from './tv-series-web';

describe('TvSeriesWeb', () => {
  let component: TvSeriesWeb;
  let fixture: ComponentFixture<TvSeriesWeb>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TvSeriesWeb]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TvSeriesWeb);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
