import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MainBlogs } from './main-blogs';

describe('MainBlogs', () => {
  let component: MainBlogs;
  let fixture: ComponentFixture<MainBlogs>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MainBlogs]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MainBlogs);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
