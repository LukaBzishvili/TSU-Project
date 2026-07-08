import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EventItem } from '../../models/models';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';
import { PortalApiService } from '../../services/portal-api';

@Component({
  selector: 'app-main',
  imports: [RouterLink, TranslatePipe],
  templateUrl: './main.html',
  styleUrl: './main.css',
})
export class Main {
  private readonly i18n = inject(I18nService);
  private readonly api = inject(PortalApiService);

  protected readonly highlightMetrics = computed(() => this.i18n.getHighlightMetrics());
  protected readonly quickLinks = computed(() => this.i18n.getQuickLinks());
  protected readonly featuredNews = computed(() => this.i18n.getNewsItems().slice(0, 3));
  protected readonly featuredEvents = signal<EventItem[]>(this.i18n.getEventItems().slice(0, 2));

  constructor() {
    this.api.getEvents().subscribe({
      next: (events) => this.featuredEvents.set(events.slice(0, 2)),
      error: () => this.featuredEvents.set(this.i18n.getEventItems().slice(0, 2)),
    });
  }
}
