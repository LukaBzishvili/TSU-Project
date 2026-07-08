import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthStateService } from '../../services/auth-state';
import { PortalApiService } from '../../services/portal-api';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';

@Component({
  selector: 'app-self-government-access',
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './self-government-access.html',
  styleUrl: './self-government-access.css',
})
export class SelfGovernmentAccess {
  private readonly api = inject(PortalApiService);
  protected readonly authState = inject(AuthStateService);
  private readonly i18n = inject(I18nService);

  protected readonly mode = signal<'login' | 'register'>('login');
  protected readonly busy = signal(false);
  protected readonly feedback = signal('');

  protected readonly registerForm = {
    email: '',
    password: '',
    organizationName: '',
    representativeName: '',
  };

  protected readonly loginForm = {
    email: '',
    password: '',
  };

  protected readonly eventForm = {
    title: '',
    date: '',
    time: '',
    location: '',
    contact: '',
    format: 'On-site',
    summary: '',
    organizer: 'Self-government',
  };

  protected setMode(mode: 'login' | 'register') {
    this.mode.set(mode);
    this.feedback.set('');
  }

  protected register() {
    this.busy.set(true);
    this.feedback.set('');

    this.api.registerSelfGovernment(this.registerForm).subscribe({
      next: (response) => {
        this.authState.setSession(response);
        this.feedback.set(response.message);
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.busy.set(false);
      },
      complete: () => this.busy.set(false),
    });
  }

  protected login() {
    this.busy.set(true);
    this.feedback.set('');

    this.api.login(this.loginForm).subscribe({
      next: (response) => {
        this.authState.setSession(response);
        this.feedback.set(response.message);
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.busy.set(false);
      },
      complete: () => this.busy.set(false),
    });
  }

  protected createEvent() {
    this.busy.set(true);
    this.feedback.set('');

    this.api.createEvent(this.eventForm).subscribe({
      next: (eventItem) => {
        this.feedback.set(`${this.i18n.t('selfGovernment.eventSaved')} ${eventItem.title}`);
        this.eventForm.title = '';
        this.eventForm.date = '';
        this.eventForm.time = '';
        this.eventForm.location = '';
        this.eventForm.contact = '';
        this.eventForm.format = 'On-site';
        this.eventForm.summary = '';
        this.eventForm.organizer = 'Self-government';
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.busy.set(false);
      },
      complete: () => this.busy.set(false),
    });
  }

  private readError(error: HttpErrorResponse) {
    return typeof error.error === 'string' ? error.error : this.i18n.t('selfGovernment.errors.requestFailed');
  }
}
