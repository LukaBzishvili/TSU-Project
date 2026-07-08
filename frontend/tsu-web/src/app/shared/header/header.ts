import { Component, signal } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthStateService } from '../../services/auth-state';
import { TranslatePipe } from '../../pipes/t.pipe';
import { AppLanguage, I18nService } from '../../services/i18n';

@Component({
  selector: 'app-header',
  imports: [RouterLink, RouterLinkActive, TranslatePipe],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  protected readonly mobileMenuOpen = signal(false);

  constructor(
    protected readonly authState: AuthStateService,
    protected readonly i18n: I18nService,
  ) {}

  protected logout() {
    this.closeMobileMenu();
    this.authState.clearSession();
  }

  protected setLanguage(language: AppLanguage) {
    this.i18n.setLanguage(language);
  }

  protected toggleMobileMenu() {
    this.mobileMenuOpen.update((isOpen) => !isOpen);
  }

  protected closeMobileMenu() {
    this.mobileMenuOpen.set(false);
  }
}
