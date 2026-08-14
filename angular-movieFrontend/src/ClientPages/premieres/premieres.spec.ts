import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Premieres } from './premieres';

describe('Premieres', () => {
  let component: Premieres;
  let fixture: ComponentFixture<Premieres>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Premieres]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Premieres);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
