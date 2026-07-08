import { Injectable, computed, signal } from '@angular/core';
import { AppSession, AuthResponse } from '../models/models';

const STORAGE_KEY = 'tsu-web-session';

@Injectable({ providedIn: 'root' })
export class AuthStateService {
  private readonly sessionState = signal<AppSession | null>(this.readStoredSession());

  readonly session = this.sessionState.asReadonly();
  readonly isAuthenticated = computed(() => this.sessionState() !== null);
  readonly role = computed(() => this.sessionState()?.role ?? null);
  readonly isStudent = computed(() => this.role() === 'Student');
  readonly isHr = computed(() => this.role() === 'HR');
  readonly isSelfGovernment = computed(() => this.role() === 'SelfGovernment');

  setSession(response: AuthResponse) {
    const session: AppSession = {
      token: response.token,
      email: response.email,
      role: response.role,
    };

    localStorage.setItem(STORAGE_KEY, JSON.stringify(session));
    this.sessionState.set(session);
  }

  clearSession() {
    localStorage.removeItem(STORAGE_KEY);
    this.sessionState.set(null);
  }

  private readStoredSession(): AppSession | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) {
      return null;
    }

    try {
      return JSON.parse(raw) as AppSession;
    } catch {
      localStorage.removeItem(STORAGE_KEY);
      return null;
    }
  }
}
