using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<JobApplication> JobApplications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<JobApplication>()
            .Property(j => j.Status).HasConversion<string>()
            .IsRequired(true);

        builder.Entity<JobApplication>()
            .HasOne(j => j.User)
            .WithMany(u => u.JobsApplied)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<JobApplication>()
            .HasOne(j => j.Company)
            .WithMany(c => c.JobsApplied)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Contact>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contacts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Contact>()
            .HasOne(contact => contact.Company)
            .WithMany(companies => companies.Contacts)
            .HasForeignKey(contact => contact.CompanyId)
            .OnDelete(DeleteBehavior.SetNull);


    }
}
