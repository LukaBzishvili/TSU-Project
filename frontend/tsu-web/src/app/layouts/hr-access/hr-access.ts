import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthStateService } from '../../services/auth-state';
import { PortalApiService } from '../../services/portal-api';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';

@Component({
  selector: 'app-hr-access',
  imports: [CommonModule, FormsModule, RouterLink, TranslatePipe],
  templateUrl: './hr-access.html',
  styleUrl: './hr-access.css',
})
export class HrAccess {
  private readonly api = inject(PortalApiService);
  private readonly authState = inject(AuthStateService);
  private readonly router = inject(Router);
  private readonly i18n = inject(I18nService);

  protected readonly mode = signal<'login' | 'register'>('register');
  protected readonly busy = signal(false);
  protected readonly feedback = signal('');

  protected readonly registerForm = {
    email: '',
    password: '',
    companyName: '',
    position: '',
    contactPhone: '',
  };

  protected readonly loginForm = {
    email: '',
    password: '',
  };

  protected setMode(mode: 'login' | 'register') {
    this.mode.set(mode);
    this.feedback.set('');
  }

  protected register() {
    this.busy.set(true);
    this.feedback.set('');

    this.api.registerHr(this.registerForm).subscribe({
      next: (response) => {
        this.authState.setSession(response);
        this.feedback.set(response.message);
        void this.router.navigateByUrl('/hr-dashboard');
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
        void this.router.navigateByUrl('/hr-dashboard');
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.busy.set(false);
      },
      complete: () => this.busy.set(false),
    });
  }

  private readError(error: HttpErrorResponse) {
    return typeof error.error === 'string' ? error.error : this.i18n.t('hrAccess.errors.requestFailed');
  }
}
