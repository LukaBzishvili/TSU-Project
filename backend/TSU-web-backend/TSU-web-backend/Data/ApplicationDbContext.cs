using Microsoft.EntityFrameworkCore;
using TSU_web_backend.Models;

namespace TSU_web_backend.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();
    public DbSet<HrProfile> HrProfiles => Set<HrProfile>();
    public DbSet<StudentExperience> StudentExperiences => Set<StudentExperience>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<StudentSkill> StudentSkills => Set<StudentSkill>();
    public DbSet<CvFile> CvFiles => Set<CvFile>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<WelcomePartyRegistration> WelcomePartyRegistrations => Set<WelcomePartyRegistration>();
    public DbSet<ClubMembershipApplication> ClubMembershipApplications => Set<ClubMembershipApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<AppUser>()
            .Property(user => user.Role)
            .HasConversion<string>();

        modelBuilder.Entity<AppUser>()
            .HasOne(user => user.StudentProfile)
            .WithOne(profile => profile.User)
            .HasForeignKey<StudentProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AppUser>()
            .HasOne(user => user.HrProfile)
            .WithOne(profile => profile.User)
            .HasForeignKey<HrProfile>(profile => profile.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudentProfile>()
            .HasMany(profile => profile.Experiences)
            .WithOne(experience => experience.StudentProfile)
            .HasForeignKey(experience => experience.StudentProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudentProfile>()
            .HasMany(profile => profile.CvFiles)
            .WithOne(file => file.StudentProfile)
            .HasForeignKey(file => file.StudentProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<StudentSkill>()
            .HasKey(studentSkill => new { studentSkill.StudentProfileId, studentSkill.SkillId });

        modelBuilder.Entity<StudentSkill>()
            .HasOne(studentSkill => studentSkill.StudentProfile)
            .WithMany(profile => profile.StudentSkills)
            .HasForeignKey(studentSkill => studentSkill.StudentProfileId);

        modelBuilder.Entity<StudentSkill>()
            .HasOne(studentSkill => studentSkill.Skill)
            .WithMany(skill => skill.StudentSkills)
            .HasForeignKey(studentSkill => studentSkill.SkillId);

        modelBuilder.Entity<Skill>()
            .HasIndex(skill => skill.Name)
            .IsUnique();

        modelBuilder.Entity<Event>()
            .HasIndex(eventItem => eventItem.Date);

        modelBuilder.Entity<WelcomePartyRegistration>()
            .HasIndex(registration => registration.TicketCode)
            .IsUnique();

        modelBuilder.Entity<ClubMembershipApplication>()
            .HasIndex(application => application.ApplicationCode)
            .IsUnique();
    }
}
