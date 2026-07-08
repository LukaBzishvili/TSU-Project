import { EventItem, HighlightMetric, NewsItem, QuickLink, StudentProfile } from '../models/models';

export const highlightMetrics: HighlightMetric[] = [
  {
    label: 'Active student opportunities',
    value: '12',
    detail: 'Research, internships, assistantships, and career workshops highlighted for CS students.',
  },
  {
    label: 'Upcoming department events',
    value: '8',
    detail: 'Talks, mentoring sessions, and student gatherings collected in one place.',
  },
  {
    label: 'Profiles ready for HR review',
    value: '34',
    detail: 'Student experience profiles can be organized into a clean, employer-friendly format.',
  },
];

export const quickLinks: QuickLink[] = [
  {
    title: 'Latest News',
    description: 'Department announcements, scholarships, competitions, and university updates.',
    available: true,
    route: '/news',
  },
  {
    title: 'Student Registration',
    description: 'Create a student account and sign in to manage your department profile.',
    available: true,
    route: '/student-access',
  },
  {
    title: 'Student Profile',
    description: 'Edit your professional details, visibility, skills, experience, and CV files.',
    available: true,
    route: '/student-profile',
  },
  {
    title: 'Events and Tickets',
    description: 'Browse department events and use the Welcome Party electronic registration flow.',
    available: true,
    route: '/events',
  },
];

export const newsItems: NewsItem[] = [
  {
    id: 1,
    title: 'Computer Science students invited to join the spring innovation challenge',
    category: 'Opportunity',
    date: '2026-04-18',
    audience: 'CS Students',
    summary:
      'A department-wide call for software, AI, and data-focused student teams to pitch practical solutions for campus and community problems.',
  },
  {
    id: 2,
    title: 'Department launches weekly career advising hours with alumni mentors',
    category: 'Career',
    date: '2026-04-16',
    audience: 'Undergraduate and MSc',
    summary:
      'Students can book short advising sessions to review portfolios, CVs, GitHub profiles, and internship plans with TSU alumni working in industry.',
  },
  {
    id: 3,
    title: 'Registration opens for research assistant placements in data science labs',
    category: 'Research',
    date: '2026-04-13',
    audience: '3rd and 4th Year',
    summary:
      'Selected students will support faculty labs, gain hands-on experience, and build project portfolios that can later be shared with employers.',
  },
  {
    id: 4,
    title: 'New student services page centralizes internship contacts and FAQ',
    category: 'Student Services',
    date: '2026-04-10',
    audience: 'All students',
    summary:
      'The department is gathering contacts, submission guidelines, and support resources into a clearer digital student hub.',
  },
];

export const eventItems: EventItem[] = [
  {
    id: 1,
    title: 'Welcome Party 2026',
    date: '2026-09-25',
    time: '18:00',
    location: 'TSU Campus Courtyard',
    contact: 'events.cs@tsu.ge',
    format: 'On-site',
    summary:
      'The main social event for incoming and returning students, with electronic ticket registration and check-in.',
  },
  {
    id: 2,
    title: 'Career Night with Georgian Tech Employers',
    date: '2026-05-08',
    time: '17:30',
    location: 'Building XI, Conference Hall',
    contact: 'career.cs@tsu.ge',
    format: 'Hybrid',
    summary:
      'Students meet recruiters, hear short company talks, and receive guidance on what employers expect from junior candidates.',
  },
  {
    id: 3,
    title: 'Open Lab: AI, Data, and Systems Research Showcase',
    date: '2026-05-20',
    time: '16:00',
    location: 'Computer Science Department Labs',
    contact: 'labs.cs@tsu.ge',
    format: 'On-site',
    summary:
      'Faculty and students present current research directions and available pathways for new student researchers.',
  },
];

export const featuredStudents: StudentProfile[] = [
  {
    id: 1,
    name: 'Ana Gelashvili',
    track: 'Software Engineering',
    graduationYear: 2027,
    headline: 'Frontend-focused CS student building accessible academic tools',
    summary:
      'Interested in product engineering, design systems, and educational technology. Looking for internship opportunities in frontend or full-stack teams.',
    skills: ['Angular', 'TypeScript', 'UI Design', 'REST APIs'],
    experience: [
      {
        role: 'Frontend Intern',
        organization: 'Campus Innovation Lab',
        period: '2025',
        description: 'Built internal dashboards for student service workflows and improved mobile responsiveness.',
      },
    ],
    portfolioUrl: 'https://portfolio.example.com/ana',
    linkedinUrl: 'https://linkedin.com/in/ana-example',
    publishedAt: '2026-04-01',
  },
  {
    id: 2,
    name: 'Luka Maisuradze',
    track: 'Data Science',
    graduationYear: 2026,
    headline: 'Machine learning student with applied analytics and research experience',
    summary:
      'Focused on Python, model evaluation, and real-world data pipelines. Interested in junior data analyst and ML engineering roles.',
    skills: ['Python', 'SQL', 'Machine Learning', 'Data Visualization'],
    experience: [
      {
        role: 'Research Assistant',
        organization: 'TSU Data Lab',
        period: '2025-2026',
        description: 'Prepared datasets, analyzed survey results, and presented findings for faculty-led research projects.',
      },
    ],
    portfolioUrl: 'https://portfolio.example.com/luka',
    linkedinUrl: 'https://linkedin.com/in/luka-example',
    publishedAt: '2026-04-05',
  },
];
