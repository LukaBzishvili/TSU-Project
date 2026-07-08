export interface NewsItem {
  id: number;
  title: string;
  category: string;
  date: string;
  summary: string;
  audience: string;
  imageUrl?: string;
}

export interface ExternalNewsItem {
  title: string;
  summary: string;
  date: string;
  category: string;
  audience: string;
  url: string;
  imageUrl: string;
  source: string;
}

export interface ClubMembershipApplicationRequest {
  fullName: string;
  email: string;
  studentId: string;
  clubName: string;
  interestArea: string;
  motivation: string;
  agreedToContact: boolean;
}

export interface ClubMembershipApplicationResponse {
  id: number;
  fullName: string;
  email: string;
  studentId: string;
  clubName: string;
  interestArea: string;
  motivation: string;
  applicationCode: string;
  status: string;
  appliedAt: string;
}

export interface EventItem {
  id: number;
  title: string;
  date: string;
  time: string;
  location: string;
  contact: string;
  format: string;
  summary: string;
  organizer?: string;
  imageUrl?: string;
}

export interface ExperienceEntry {
  role: string;
  organization: string;
  period: string;
  description: string;
}

export interface StudentProfile {
  id: number;
  name: string;
  track: string;
  graduationYear: number;
  headline: string;
  summary: string;
  skills: string[];
  experience: ExperienceEntry[];
  portfolioUrl: string;
  linkedinUrl: string;
  publishedAt: string;
}

export interface TicketRegistration {
  fullName: string;
  studentId: string;
  email: string;
  ticketType: string;
  guestCount: number;
  notes: string;
  acceptedTerms: boolean;
}

export interface TicketConfirmation {
  fullName: string;
  ticketCode: string;
  ticketType: string;
  issuedAt: string;
  guestCount: number;
}

export interface WelcomePartyRegistrationRequest {
  fullName: string;
  email: string;
  studentId: string;
  faculty: string;
  guestCount: number;
  notes: string;
  agreed: boolean;
}

export interface WelcomePartyRegistrationResponse {
  id: number;
  fullName: string;
  email: string;
  studentId: string;
  faculty: string;
  guestCount: number;
  notes: string;
  ticketCode: string;
  status: string;
  registeredAt: string;
}

export interface HighlightMetric {
  label: string;
  value: string;
  detail: string;
}

export interface QuickLink {
  title: string;
  description: string;
  available: boolean;
  route?: string;
  imageUrl?: string;
}

export interface AppSession {
  token: string;
  email: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  role: string;
  message: string;
}

export interface StudentRegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  studentIdNumber: string;
  graduationYear: number;
}

export interface HrRegisterRequest {
  email: string;
  password: string;
  companyName: string;
  position: string;
  contactPhone: string;
}

export interface SelfGovernmentRegisterRequest {
  email: string;
  password: string;
  organizationName: string;
  representativeName: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface CreateEventRequest {
  title: string;
  date: string;
  time: string;
  location: string;
  contact: string;
  format: string;
  summary: string;
  organizer: string;
}

export interface StudentExperienceRequest {
  title: string;
  organization: string;
  period: string;
  description: string;
}

export interface UpdateStudentProfileRequest {
  firstName: string;
  lastName: string;
  studentIdNumber: string;
  department: string;
  graduationYear: number;
  summary: string;
  linkedInUrl: string | null;
  gitHubUrl: string | null;
  portfolioUrl: string | null;
  phone: string | null;
  isVisibleToHr: boolean;
  skills: string[];
  experiences: StudentExperienceRequest[];
}

export interface CvFileResponse {
  id: number;
  originalFileName: string;
  storedFileName: string;
  filePath: string;
  uploadedAt: string;
}

export interface StudentProfileResponse {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  studentIdNumber: string;
  department: string;
  graduationYear: number;
  summary: string;
  linkedInUrl: string | null;
  gitHubUrl: string | null;
  portfolioUrl: string | null;
  phone: string | null;
  isVisibleToHr: boolean;
  skills: string[];
  experiences: StudentExperienceRequest[];
  cvFiles: CvFileResponse[];
}

export interface HrStudentCardResponse {
  profileId: number;
  fullName: string;
  email: string;
  department: string;
  graduationYear: number;
  summary: string;
  skills: string[];
  experiences: StudentExperienceRequest[];
  cvFiles: CvFileResponse[];
}
