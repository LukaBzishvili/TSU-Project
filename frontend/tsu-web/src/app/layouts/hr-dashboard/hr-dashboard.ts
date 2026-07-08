import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthStateService } from '../../services/auth-state';
import { PortalApiService } from '../../services/portal-api';
import { HrStudentCardResponse } from '../../models/models';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';

@Component({
  selector: 'app-hr-dashboard',
  imports: [CommonModule, FormsModule, RouterLink, TranslatePipe],
  templateUrl: './hr-dashboard.html',
  styleUrl: './hr-dashboard.css',
})
export class HrDashboard {
  private readonly api = inject(PortalApiService);
  private readonly i18n = inject(I18nService);
  protected readonly authState = inject(AuthStateService);

  protected readonly loading = signal(false);
  protected readonly feedback = signal('');
  protected readonly students = signal<HrStudentCardResponse[]>([]);
  protected readonly search = signal('');
  protected readonly graduationFilter = signal('all');

  protected readonly graduationOptions = computed(() =>
    Array.from(new Set(this.students().map((student) => student.graduationYear.toString()))).sort(),
  );

  protected readonly filteredStudents = computed(() => {
    const searchTerm = this.search().trim().toLowerCase();
    const selectedYear = this.graduationFilter();

    return this.students().filter((student) => {
      const matchesYear = selectedYear === 'all' || student.graduationYear.toString() === selectedYear;
      const matchesSearch =
        !searchTerm ||
        student.fullName.toLowerCase().includes(searchTerm) ||
        student.email.toLowerCase().includes(searchTerm) ||
        student.department.toLowerCase().includes(searchTerm) ||
        student.skills.some((skill) => skill.toLowerCase().includes(searchTerm));

      return matchesYear && matchesSearch;
    });
  });

  constructor() {
    if (this.authState.isHr()) {
      this.loadStudents();
    }
  }

  protected loadStudents() {
    this.loading.set(true);
    this.feedback.set('');

    this.api.getVisibleStudents().subscribe({
      next: (students) => {
        this.students.set(students);
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.loading.set(false);
      },
      complete: () => this.loading.set(false),
    });
  }

  protected resetFilters() {
    this.search.set('');
    this.graduationFilter.set('all');
  }

  private readError(error: HttpErrorResponse) {
    if (error.status === 403) {
      return this.i18n.t('hrDashboard.errors.forbidden');
    }

    return typeof error.error === 'string' ? error.error : this.i18n.t('hrDashboard.errors.loadFailed');
  }
}
