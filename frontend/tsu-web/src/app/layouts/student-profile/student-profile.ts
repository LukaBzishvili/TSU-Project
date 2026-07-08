import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthStateService } from '../../services/auth-state';
import { PortalApiService } from '../../services/portal-api';
import { StudentExperienceRequest, StudentProfileResponse, UpdateStudentProfileRequest } from '../../models/models';
import { TranslatePipe } from '../../pipes/t.pipe';
import { I18nService } from '../../services/i18n';

const blankExperience = (): StudentExperienceRequest => ({
  title: '',
  organization: '',
  period: '',
  description: '',
});

@Component({
  selector: 'app-student-profile',
  imports: [CommonModule, FormsModule, RouterLink, TranslatePipe],
  templateUrl: './student-profile.html',
  styleUrl: './student-profile.css',
})
export class StudentProfileLayout {
  private readonly api = inject(PortalApiService);
  private readonly i18n = inject(I18nService);
  protected readonly authState = inject(AuthStateService);

  protected readonly loading = signal(false);
  protected readonly saving = signal(false);
  protected readonly feedback = signal('');
  protected readonly selectedFileName = signal('');
  protected readonly profile = signal<StudentProfileResponse | null>(null);

  protected readonly formModel: UpdateStudentProfileRequest = {
    firstName: '',
    lastName: '',
    studentIdNumber: '',
    department: 'Computer Science',
    graduationYear: new Date().getFullYear() + 1,
    summary: '',
    linkedInUrl: null,
    gitHubUrl: null,
    portfolioUrl: null,
    phone: null,
    isVisibleToHr: false,
    skills: [],
    experiences: [blankExperience()],
  };

  protected skillsText = '';

  constructor() {
    if (this.authState.isStudent()) {
      this.loadProfile();
    }
  }

  protected loadProfile() {
    this.loading.set(true);
    this.feedback.set('');

    this.api.getMyProfile().subscribe({
      next: (profile) => {
        this.profile.set(profile);
        this.patchForm(profile);
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.loading.set(false);
      },
      complete: () => this.loading.set(false),
    });
  }

  protected addExperience() {
    this.formModel.experiences.push(blankExperience());
  }

  protected removeExperience(index: number) {
    if (this.formModel.experiences.length === 1) {
      this.formModel.experiences[0] = blankExperience();
      return;
    }

    this.formModel.experiences.splice(index, 1);
  }

  protected saveProfile() {
    this.saving.set(true);
    this.feedback.set('');
    this.formModel.skills = this.skillsText
      .split(',')
      .map((skill) => skill.trim())
      .filter(Boolean);

    this.api.updateMyProfile(this.formModel).subscribe({
      next: (profile) => {
        this.profile.set(profile);
        this.patchForm(profile);
        this.feedback.set(this.i18n.t('studentProfile.errors.saveSuccess'));
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
        this.saving.set(false);
      },
      complete: () => this.saving.set(false),
    });
  }

  protected onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    if (!file) {
      return;
    }

    this.selectedFileName.set(file.name);
    this.api.uploadCv(file).subscribe({
      next: (uploadedFile) => {
        const profile = this.profile();
        if (profile) {
          this.profile.set({
            ...profile,
            cvFiles: [uploadedFile, ...profile.cvFiles],
          });
        }
        this.feedback.set(`${this.i18n.t('studentProfile.errors.uploadSuccess')} ${uploadedFile.originalFileName}.`);
      },
      error: (error: HttpErrorResponse) => {
        this.feedback.set(this.readError(error));
      },
    });
  }

  private patchForm(profile: StudentProfileResponse) {
    this.formModel.firstName = profile.firstName;
    this.formModel.lastName = profile.lastName;
    this.formModel.studentIdNumber = profile.studentIdNumber;
    this.formModel.department = profile.department;
    this.formModel.graduationYear = profile.graduationYear;
    this.formModel.summary = profile.summary;
    this.formModel.linkedInUrl = profile.linkedInUrl;
    this.formModel.gitHubUrl = profile.gitHubUrl;
    this.formModel.portfolioUrl = profile.portfolioUrl;
    this.formModel.phone = profile.phone;
    this.formModel.isVisibleToHr = profile.isVisibleToHr;
    this.formModel.skills = [...profile.skills];
    this.formModel.experiences = profile.experiences.length ? [...profile.experiences] : [blankExperience()];
    this.skillsText = profile.skills.join(', ');
  }

  private readError(error: HttpErrorResponse) {
    return typeof error.error === 'string' ? error.error : this.i18n.t('studentProfile.errors.requestFailed');
  }
}
