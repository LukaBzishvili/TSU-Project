using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TSU_web_backend.Models;

namespace TSU_web_backend.Data;

public static class DevelopmentDataSeeder
{
    private const string DemoPassword = "Password123!";

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();

        var passwordHasher = new PasswordHasher<AppUser>();

        await SeedApprovedHrAsync(dbContext, passwordHasher);
        await SeedSelfGovernmentAsync(dbContext, passwordHasher);
        await ApproveLocalHrAccountsAsync(dbContext);
        await SeedStudentsAsync(dbContext, passwordHasher);
        await SeedEventsAsync(dbContext);

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedApprovedHrAsync(ApplicationDbContext dbContext, PasswordHasher<AppUser> passwordHasher)
    {
        const string email = "hr.demo@tsu.ge";

        var existingUser = await dbContext.Users
            .Include(user => user.HrProfile)
            .FirstOrDefaultAsync(user => user.Email == email);

        if (existingUser?.HrProfile is not null)
        {
            existingUser.PasswordHash = passwordHasher.HashPassword(existingUser, DemoPassword);
            existingUser.HrProfile.IsApproved = true;
            return;
        }

        var user = new AppUser
        {
            Email = email,
            Role = UserRole.HR,
            HrProfile = new HrProfile
            {
                CompanyName = "TSU Career Partners",
                Position = "Recruitment Manager",
                ContactPhone = "+995 555 010 202",
                IsApproved = true
            }
        };

        user.PasswordHash = passwordHasher.HashPassword(user, DemoPassword);
        dbContext.Users.Add(user);
    }

    private static async Task ApproveLocalHrAccountsAsync(ApplicationDbContext dbContext)
    {
        var pendingHrProfiles = await dbContext.HrProfiles
            .Where(profile => !profile.IsApproved)
            .ToListAsync();

        foreach (var profile in pendingHrProfiles)
        {
            profile.IsApproved = true;
        }
    }

    private static async Task SeedSelfGovernmentAsync(ApplicationDbContext dbContext, PasswordHasher<AppUser> passwordHasher)
    {
        const string email = "selfgov.demo@tsu.ge";

        var existingUser = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
        if (existingUser is not null)
        {
            existingUser.PasswordHash = passwordHasher.HashPassword(existingUser, DemoPassword);
            existingUser.Role = UserRole.SelfGovernment;
            return;
        }

        var user = new AppUser
        {
            Email = email,
            Role = UserRole.SelfGovernment
        };

        user.PasswordHash = passwordHasher.HashPassword(user, DemoPassword);
        dbContext.Users.Add(user);
    }

    private static async Task SeedStudentsAsync(ApplicationDbContext dbContext, PasswordHasher<AppUser> passwordHasher)
    {
        var seedStudents = new[]
        {
            new SeedStudent(
                "nino.kapanadze@student.tsu.ge",
                "Nino",
                "Kapanadze",
                "CS-2026-001",
                2026,
                "Computer Science student focused on frontend engineering, accessibility, and clean product interfaces.",
                "https://github.com/ninokapanadze",
                new[] { "Angular", "TypeScript", "HTML/CSS", "Accessibility" },
                new[]
                {
                    new SeedExperience("Frontend Intern", "Tbilisi Digital Lab", "2025 Summer", "Built reusable Angular components and improved form accessibility for an internal dashboard."),
                    new SeedExperience("Student Project Lead", "TSU CS Club", "2024 - 2025", "Led a small team building a student event registration prototype.")
                }),
            new SeedStudent(
                "giorgi.maislashvili@student.tsu.ge",
                "Giorgi",
                "Maislashvili",
                "CS-2025-014",
                2025,
                "Backend-oriented student interested in APIs, databases, and reliable student service platforms.",
                "https://github.com/giorgimaislashvili",
                new[] { "C#", "ASP.NET Core", "SQL Server", "REST APIs" },
                new[]
                {
                    new SeedExperience("Backend Developer Intern", "Campus Systems Group", "2025", "Created API endpoints for registration workflows and wrote validation logic for student records."),
                    new SeedExperience("Database Assistant", "TSU Research Lab", "2024", "Helped normalize research participant data and prepared SQL reports for faculty supervisors.")
                }),
            new SeedStudent(
                "mariam.lomidze@student.tsu.ge",
                "Mariam",
                "Lomidze",
                "CS-2027-022",
                2027,
                "Data and AI enthusiast with experience preparing datasets, visualizations, and small machine learning demos.",
                "https://github.com/mariamlomidze",
                new[] { "Python", "Data Analysis", "Machine Learning", "Power BI" },
                new[]
                {
                    new SeedExperience("Data Analyst Volunteer", "Student Research Showcase", "2025", "Cleaned survey data and created charts summarizing student participation trends."),
                    new SeedExperience("AI Workshop Mentor", "TSU CS Club", "2025", "Supported beginner students during Python and machine learning practice sessions.")
                })
        };

        foreach (var seedStudent in seedStudents)
        {
            var existingUser = await dbContext.Users
                .Include(user => user.StudentProfile)
                .FirstOrDefaultAsync(user => user.Email == seedStudent.Email);

            if (existingUser is not null)
            {
                existingUser.PasswordHash = passwordHasher.HashPassword(existingUser, DemoPassword);

                if (existingUser.StudentProfile is not null)
                {
                    existingUser.StudentProfile.IsVisibleToHr = true;
                }

                continue;
            }

            var user = new AppUser
            {
                Email = seedStudent.Email,
                Role = UserRole.Student,
                StudentProfile = new StudentProfile
                {
                    FirstName = seedStudent.FirstName,
                    LastName = seedStudent.LastName,
                    StudentIdNumber = seedStudent.StudentIdNumber,
                    Department = "Computer Science",
                    GraduationYear = seedStudent.GraduationYear,
                    Summary = seedStudent.Summary,
                    GitHubUrl = seedStudent.GitHubUrl,
                    IsVisibleToHr = true,
                    Experiences = seedStudent.Experiences
                        .Select(experience => new StudentExperience
                        {
                            Title = experience.Title,
                            Organization = experience.Organization,
                            Period = experience.Period,
                            Description = experience.Description
                        })
                        .ToList()
                }
            };

            user.PasswordHash = passwordHasher.HashPassword(user, DemoPassword);

            foreach (var skillName in seedStudent.Skills)
            {
                var skill = await dbContext.Skills.FirstOrDefaultAsync(existingSkill => existingSkill.Name == skillName)
                    ?? new Skill { Name = skillName };

                user.StudentProfile.StudentSkills.Add(new StudentSkill
                {
                    StudentProfile = user.StudentProfile,
                    Skill = skill
                });
            }

            dbContext.Users.Add(user);
        }
    }

    private static async Task SeedEventsAsync(ApplicationDbContext dbContext)
    {
        if (await dbContext.Events.AnyAsync())
        {
            return;
        }

        dbContext.Events.AddRange(
            new Event
            {
                Title = "Welcome Party 2026",
                Date = new DateOnly(2026, 9, 25),
                Time = "18:00",
                Location = "TSU Campus Courtyard",
                Contact = "events.cs@tsu.ge",
                Format = "On-site",
                Summary = "The main social event for incoming and returning students, with electronic ticket registration and check-in.",
                Organizer = "Computer Science Department"
            },
            new Event
            {
                Title = "Career Night with Georgian Tech Employers",
                Date = new DateOnly(2026, 5, 8),
                Time = "17:30",
                Location = "Building XI, Conference Hall",
                Contact = "career.cs@tsu.ge",
                Format = "Hybrid",
                Summary = "Students meet recruiters, hear short company talks, and receive guidance on what employers expect from junior candidates.",
                Organizer = "Computer Science Department"
            },
            new Event
            {
                Title = "Open Lab: AI, Data, and Systems Research Showcase",
                Date = new DateOnly(2026, 5, 20),
                Time = "16:00",
                Location = "Computer Science Department Labs",
                Contact = "labs.cs@tsu.ge",
                Format = "On-site",
                Summary = "Faculty and students present current research directions and available pathways for new student researchers.",
                Organizer = "Computer Science Department"
            },
            new Event
            {
                Title = "Student Self-government Project Fair",
                Date = new DateOnly(2026, 6, 12),
                Time = "15:00",
                Location = "TSU Student Space",
                Contact = "selfgov@tsu.ge",
                Format = "On-site",
                Summary = "Student teams present club projects, volunteer initiatives, and collaboration opportunities for the next semester.",
                Organizer = "Self-government"
            });
    }

    private sealed record SeedStudent(
        string Email,
        string FirstName,
        string LastName,
        string StudentIdNumber,
        int GraduationYear,
        string Summary,
        string GitHubUrl,
        IEnumerable<string> Skills,
        IEnumerable<SeedExperience> Experiences);

    private sealed record SeedExperience(
        string Title,
        string Organization,
        string Period,
        string Description);
}
