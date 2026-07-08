import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthStateService } from '../../services/auth-state';
import { TranslatePipe } from '../../pipes/t.pipe';
@Component({
  selector: 'app-footer',
  imports: [RouterLink, TranslatePipe],
  templateUrl: './footer.html',
  styleUrl: './footer.css',
})
export class Footer {
  constructor(protected readonly authState: AuthStateService) {}
}
