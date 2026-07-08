import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EventItem, NewsItem, QuickLink } from '../../models/models';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';
import { PortalApiService } from '../../services/portal-api';

const QUICK_LINK_IMAGES = [
  'https://www.computing.tsu.ge/upload/news/7BOV9yuIZucrYMqk.jpg',
  'https://www.computing.tsu.ge/upload/news/K0ywZjWRsWPo7EM5.jpg',
  'https://www.computing.tsu.ge/upload/news/jKrhlL9ULXCp8tNr.jpg',
  'https://www.computing.tsu.ge/upload/news/dqD92OD3YTmgwVLd.jpg',
];

const FEATURED_NEWS_IMAGES = [
  'https://www.computing.tsu.ge/upload/news/7BOV9yuIZucrYMqk.jpg',
  'https://www.computing.tsu.ge/upload/news/K0ywZjWRsWPo7EM5.jpg',
  'https://www.computing.tsu.ge/upload/news/laM8kgavtvy8hVAa.jpg',
];

const FEATURED_EVENT_IMAGES = [
  'https://www.computing.tsu.ge/upload/news/dqD92OD3YTmgwVLd.jpg',
  'https://www.computing.tsu.ge/upload/news/xozYlUpc0MzWBFFj.JPG',
  'https://www.computing.tsu.ge/upload/news/qgcdLKHcbv10C2Mm.jpg',
];

@Component({
  selector: 'app-main',
  imports: [RouterLink, TranslatePipe],
  templateUrl: './main.html',
  styleUrl: './main.css',
})
export class Main {
  private readonly i18n = inject(I18nService);
  private readonly api = inject(PortalApiService);
  protected readonly spotlightImage = 'https://www.computing.tsu.ge/upload/news/PSl6BYimgdTEu7k7.png';

  protected readonly highlightMetrics = computed(() => this.i18n.getHighlightMetrics());
  protected readonly quickLinks = computed(() =>
    this.i18n.getQuickLinks().map((link, index): QuickLink => ({
      ...link,
      imageUrl: QUICK_LINK_IMAGES[index % QUICK_LINK_IMAGES.length],
    })),
  );
  protected readonly featuredNews = computed(() =>
    this.i18n.getNewsItems().slice(0, 3).map((item, index): NewsItem => ({
      ...item,
      imageUrl: FEATURED_NEWS_IMAGES[index % FEATURED_NEWS_IMAGES.length],
    })),
  );
  protected readonly featuredEvents = signal<EventItem[]>(this.i18n.getEventItems().slice(0, 2));

  constructor() {
    this.api.getEvents().subscribe({
      next: (events) =>
        this.featuredEvents.set(
          events.slice(0, 2).map((event, index) => ({
            ...event,
            imageUrl: FEATURED_EVENT_IMAGES[index % FEATURED_EVENT_IMAGES.length],
          })),
        ),
      error: () =>
        this.featuredEvents.set(
          this.i18n.getEventItems().slice(0, 2).map((event, index) => ({
            ...event,
            imageUrl: FEATURED_EVENT_IMAGES[index % FEATURED_EVENT_IMAGES.length],
          })),
        ),
    });
  }
}
