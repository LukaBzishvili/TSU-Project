import { Routes } from '@angular/router';
import { HrAccess } from './layouts/hr-access/hr-access';
import { HrDashboard } from './layouts/hr-dashboard/hr-dashboard';
import { Main } from './layouts/main/main';
import { NewsLayout } from './layouts/news/news';
import { EventsLayout } from './layouts/events/events';
import { ClubMembershipLayout } from './layouts/club-membership/club-membership';
import { StudentAccess } from './layouts/student-access/student-access';
import { StudentProfileLayout } from './layouts/student-profile/student-profile';
import { SelfGovernmentAccess } from './layouts/self-government-access/self-government-access';
import { WelcomePartyLayout } from './layouts/welcome-party/welcome-party';

export const routes: Routes = [
  {
    path: '',
    component: Main,
  },
  {
    path: 'news',
    component: NewsLayout,
  },
  {
    path: 'events',
    component: EventsLayout,
  },
  {
    path: 'club-membership',
    component: ClubMembershipLayout,
  },
  {
    path: 'student-access',
    component: StudentAccess,
  },
  {
    path: 'student-profile',
    component: StudentProfileLayout,
  },
  {
    path: 'hr-access',
    component: HrAccess,
  },
  {
    path: 'hr-dashboard',
    component: HrDashboard,
  },
  {
    path: 'self-government',
    component: SelfGovernmentAccess,
  },
  {
    path: 'welcome-party',
    component: WelcomePartyLayout,
  },
];
