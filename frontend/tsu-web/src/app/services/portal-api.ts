import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import {
  AuthResponse,
  ClubMembershipApplicationRequest,
  ClubMembershipApplicationResponse,
  CreateEventRequest,
  CvFileResponse,
  EventItem,
  ExternalNewsItem,
  HrRegisterRequest,
  HrStudentCardResponse,
  LoginRequest,
  SelfGovernmentRegisterRequest,
  StudentProfileResponse,
  StudentRegisterRequest,
  UpdateStudentProfileRequest,
  WelcomePartyRegistrationRequest,
  WelcomePartyRegistrationResponse,
} from '../models/models';
import { AuthStateService } from './auth-state';

@Injectable({ providedIn: 'root' })
export class PortalApiService {
  private readonly http = inject(HttpClient);
  private readonly authState = inject(AuthStateService);
  private readonly baseUrl = 'http://localhost:5213/api';

  registerStudent(payload: StudentRegisterRequest) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/Auth/register/student`, payload);
  }

  registerHr(payload: HrRegisterRequest) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/Auth/register/hr`, payload);
  }

  registerSelfGovernment(payload: SelfGovernmentRegisterRequest) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/Auth/register/self-government`, payload);
  }

  login(payload: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.baseUrl}/Auth/login`, payload);
  }

  getEvents() {
    return this.http.get<EventItem[]>(`${this.baseUrl}/Events`);
  }

  createEvent(payload: CreateEventRequest) {
    return this.http.post<EventItem>(`${this.baseUrl}/Events`, payload, {
      headers: this.getAuthHeaders(),
    });
  }

  getMyProfile() {
    return this.http.get<StudentProfileResponse>(`${this.baseUrl}/Students/me`, {
      headers: this.getAuthHeaders(),
    });
  }

  updateMyProfile(payload: UpdateStudentProfileRequest) {
    return this.http.put<StudentProfileResponse>(`${this.baseUrl}/Students/me`, payload, {
      headers: this.getAuthHeaders(),
    });
  }

  uploadCv(file: File) {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<CvFileResponse>(`${this.baseUrl}/Students/me/cv`, formData, {
      headers: this.getAuthHeaders(),
    });
  }

  getVisibleStudents() {
    return this.http.get<HrStudentCardResponse[]>(`${this.baseUrl}/Hr/students`, {
      headers: this.getAuthHeaders(),
    });
  }

  registerWelcomeParty(payload: WelcomePartyRegistrationRequest) {
    return this.http.post<WelcomePartyRegistrationResponse>(`${this.baseUrl}/WelcomeParty/register`, payload);
  }

  getTsuNews() {
    return this.http.get<ExternalNewsItem[]>(`${this.baseUrl}/News/tsu`);
  }

  applyForClubMembership(payload: ClubMembershipApplicationRequest) {
    return this.http.post<ClubMembershipApplicationResponse>(`${this.baseUrl}/ClubMembership/apply`, payload);
  }

  private getAuthHeaders() {
    const token = this.authState.session()?.token ?? '';
    return new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
  }
}
