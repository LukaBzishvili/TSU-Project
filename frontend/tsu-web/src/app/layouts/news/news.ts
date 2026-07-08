import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { TranslatePipe } from '../../pipes/t.pipe';
import { PortalApiService } from '../../services/portal-api';
import { ExternalNewsItem } from '../../models/models';

@Component({
  selector: 'app-news-layout',
  imports: [CommonModule, TranslatePipe],
  templateUrl: './news.html',
  styleUrl: './news.css',
})
export class NewsLayout {
  private readonly api = inject(PortalApiService);

  protected readonly loading = signal(true);
  protected readonly feedback = signal('');
  protected readonly newsItems = signal<ExternalNewsItem[]>([]);

  constructor() {
    this.loadNews();
  }

  protected loadNews() {
    this.loading.set(true);
    this.feedback.set('');

    this.api.getTsuNews().subscribe({
      next: (items) => {
        this.newsItems.set(items.slice(1));
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(typeof error.error === 'string' ? error.error : 'Could not load official TSU news.');
        this.loading.set(false);
      },
      complete: () => this.loading.set(false),
    });
  }
}
