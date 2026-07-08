import { Component } from '@angular/core';

@Component({
  selector: 'app-news-card',
  imports: [],
  templateUrl: './news-card.html',
  styleUrl: './news-card.css',
})
export class NewsCard {
  text =
    'Lorem ipsum dolor sit amet, consectetur adipisicing elit. Porro distinctio consequuntur corporis cupiditate quasi error deserunt. Eos veritatis, impedit tempora ipsam repudiandae aliquid vero quam, eveniet quaerat corporis distinctio beatae.';

  cutLongText(text: string) {
    if (text.length > 80) {
      return text.slice(0, 80) + '...';
    }
    return text;
  }
}
