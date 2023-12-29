using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sharc.Core.Entities;

namespace Sharc.Server.Data.Persistence; 

public class ApplicationDbContext : DbContext {
    
    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions opt) : base(opt) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Event>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<User>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<Event>()
            .HasMany<User>(e => e.Attendees)
            .WithMany();
        
        ConfigureEventProperties(modelBuilder);
        ConfigureUserProperties(modelBuilder);
    }

    private void ConfigureUserProperties(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>()
            .Property(e => e.CachedUsername)
            .IsRequired()
            .HasMaxLength(36);

        modelBuilder.Entity<User>()
            .Property(e => e.Email)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(e => e.IdentityId)
            .IsRequired();
    }

    private void ConfigureRecurrenceRuleProperties(OwnedNavigationBuilder<Event, RecurrenceRule> builder) {
        builder
            .Property(e => e.Frequency)
            .IsRequired();
    }
    
    private void ConfigureEventProperties(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Event>()
            .OwnsOne<RecurrenceRule>(e => e.RecurrenceRule, ConfigureRecurrenceRuleProperties );

        
        modelBuilder.Entity<Event>()
            .Property(e => e.Created)
            .ValueGeneratedOnAddOrUpdate();

        modelBuilder.Entity<Event>()
            .Property(e => e.Description)
            .HasMaxLength(255);

        modelBuilder.Entity<Event>()
            .Property(e => e.Summary)
            .IsRequired()
            .HasMaxLength(128);

        modelBuilder.Entity<Event>()
            .Property(e => e.EndTime)
            .IsRequired();
        modelBuilder.Entity<Event>()
            .Property(e => e.StartTime)
            .IsRequired();
    }
}