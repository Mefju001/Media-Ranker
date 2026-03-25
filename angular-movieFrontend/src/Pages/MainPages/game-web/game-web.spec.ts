import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameWeb } from './game-web';

describe('GameWeb', () => {
  let component: GameWeb;
  let fixture: ComponentFixture<GameWeb>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GameWeb]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameWeb);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
