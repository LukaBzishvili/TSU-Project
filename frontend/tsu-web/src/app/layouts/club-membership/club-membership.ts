import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ClubMembershipApplicationResponse } from '../../models/models';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';
import { PortalApiService } from '../../services/portal-api';

@Component({
  selector: 'app-club-membership-layout',
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './club-membership.html',
  styleUrl: './club-membership.css',
})
export class ClubMembershipLayout {
  private readonly api = inject(PortalApiService);
  private readonly i18n = inject(I18nService);

  protected readonly submitting = signal(false);
  protected readonly feedback = signal('');
  protected readonly application = signal<ClubMembershipApplicationResponse | null>(null);

  protected readonly form = {
    fullName: '',
    email: '',
    studentId: '',
    clubName: 'TSU CS Club',
    interestArea: '',
    motivation: '',
    agreedToContact: false,
  };

  protected readonly previewCode = computed(() =>
    `TSU-CLUB-${this.form.studentId || 'PENDING'}-${Math.max(this.form.clubName.length, 1)}`.toUpperCase(),
  );

  protected submit() {
    this.submitting.set(true);
    this.feedback.set('');

    this.api.applyForClubMembership(this.form).subscribe({
      next: (response) => {
        this.application.set(response);
        this.feedback.set(this.i18n.t('clubMembership.errors.saveSuccess'));
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.submitting.set(false);
      },
      complete: () => this.submitting.set(false),
    });
  }

  private readError(error: HttpErrorResponse) {
    return typeof error.error === 'string'
      ? error.error
      : this.i18n.t('clubMembership.errors.requestFailed');
  }
}
