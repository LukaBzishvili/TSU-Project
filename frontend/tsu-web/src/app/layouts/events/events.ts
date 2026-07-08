import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EventItem } from '../../models/models';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';
import { PortalApiService } from '../../services/portal-api';

@Component({
  selector: 'app-events-layout',
  imports: [RouterLink, TranslatePipe],
  templateUrl: './events.html',
  styleUrl: './events.css',
})
export class EventsLayout {
  private readonly i18n = inject(I18nService);
  private readonly api = inject(PortalApiService);

  protected readonly eventItems = signal<EventItem[]>(this.i18n.getEventItems());
  protected readonly loadError = signal('');

  constructor() {
    this.api.getEvents().subscribe({
      next: (events) => {
        this.eventItems.set(events.length ? events : this.i18n.getEventItems());
        this.loadError.set('');
      },
      error: () => {
        this.loadError.set(this.i18n.t('eventsPage.loadFailed'));
      },
    });
  }
}
