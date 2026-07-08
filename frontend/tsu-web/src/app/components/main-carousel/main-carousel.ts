import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { CarouselComponent } from 'ngx-carousel-ease';
// import { MainCarouselSlideProps } from '../../models/models';

@Component({
  selector: 'app-main-carousel',
  standalone: true,
  imports: [CarouselComponent, CommonModule],
  templateUrl: './main-carousel.html',
  styleUrls: ['./main-carousel.css'],
})
export class MainCarousel {
  // slides: MainCarouselSlideProps[] = [];
  slides = ['Slide 1', 'Slide 2', 'Slide 3', 'Slide 4'];

  carouselOptions = {
    slideToShow: 1,
    slideToScroll: 1,
    infinite: true,
    dots: true,
    arrows: true,
    autoPlay: true,
    autoPlaySpeed: 3000,
    speed: 600,
  };
}
