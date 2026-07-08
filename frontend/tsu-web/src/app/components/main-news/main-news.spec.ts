import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainNews } from './main-news';

describe('MainNews', () => {
  let component: MainNews;
  let fixture: ComponentFixture<MainNews>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainNews]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainNews);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
