import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PortalApiService } from '../../services/portal-api';
import { WelcomePartyRegistrationResponse } from '../../models/models';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';

@Component({
  selector: 'app-welcome-party-layout',
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './welcome-party.html',
  styleUrl: './welcome-party.css',
})
export class WelcomePartyLayout {
  private readonly api = inject(PortalApiService);
  private readonly i18n = inject(I18nService);

  protected readonly submitting = signal(false);
  protected readonly feedback = signal('');
  protected readonly registration = signal<WelcomePartyRegistrationResponse | null>(null);
  protected readonly form = {
    fullName: '',
    email: '',
    studentId: '',
    faculty: 'Computer Science',
    agreed: false,
  };

  protected readonly previewCode = computed(() =>
    `TSU-WP-${this.form.studentId || 'PENDING'}-${Math.max(this.form.fullName.length, 1)}`.toUpperCase(),
  );

  protected readonly qrCodeUrl = computed(() => {
    const ticketCode = this.registration()?.ticketCode || this.previewCode();
    return `https://api.qrserver.com/v1/create-qr-code/?size=220x220&data=${encodeURIComponent(ticketCode)}`;
  });

  protected submit() {
    this.submitting.set(true);
    this.feedback.set('');

    this.api.registerWelcomeParty(this.form).subscribe({
      next: (response) => {
        this.registration.set(response);
        this.feedback.set(this.i18n.t('welcomeParty.errors.saveSuccess'));
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.submitting.set(false);
      },
      complete: () => this.submitting.set(false),
    });
  }

  private readError(error: HttpErrorResponse) {
    return typeof error.error === 'string' ? error.error : this.i18n.t('welcomeParty.errors.requestFailed');
  }
}
