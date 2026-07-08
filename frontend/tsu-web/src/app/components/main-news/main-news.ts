import { Component } from '@angular/core';
import { NewsCard } from '../news-card/news-card';
import { CommonModule } from '@angular/common';
// import { NewsCardProps } from '../../models/models';

@Component({
  selector: 'app-main-news',
  imports: [NewsCard, CommonModule],
  templateUrl: './main-news.html',
  styleUrls: ['./main-news.css'],
})
export class MainNews {
  // slides: NewsCardProps[] = []
  slides = ['Slide 1', 'Slide 2', 'Slide 3', 'Slide 4', 'Slide 5', 'Slide 6', 'Slide 7', 'Slide 8'];
}
