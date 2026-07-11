import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovieWeb } from './movie-web';

describe('MovieWeb', () => {
  let component: MovieWeb;
  let fixture: ComponentFixture<MovieWeb>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MovieWeb]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MovieWeb);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
