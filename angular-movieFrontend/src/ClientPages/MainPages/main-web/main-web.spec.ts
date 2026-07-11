import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainWeb } from './main-web';

describe('MainWeb', () => {
  let component: MainWeb;
  let fixture: ComponentFixture<MainWeb>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainWeb]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainWeb);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
