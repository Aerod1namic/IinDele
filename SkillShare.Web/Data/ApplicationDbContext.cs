using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillShare.Web.Models;

namespace SkillShare.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Skill> Skills { get; set; }
        public DbSet<Collaboration> Collaborations { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.TeachingSkills)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserTeachingSkills"));

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.LearningSkills)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserLearningSkills"));

            builder.Entity<Collaboration>()
                .HasOne(c => c.Initiator)
                .WithMany()
                .HasForeignKey(c => c.InitiatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Collaboration>()
                .HasOne(c => c.Target)
                .WithMany()
                .HasForeignKey(c => c.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Collaboration>()
                .HasMany(c => c.InitiatorTeachingSkills)
                .WithMany()
                .UsingEntity(j => j.ToTable("CollaborationInitiatorTeachingSkills"));

            builder.Entity<Collaboration>()
                .HasMany(c => c.InitiatorLearningSkills)
                .WithMany()
                .UsingEntity(j => j.ToTable("CollaborationInitiatorLearningSkills"));
        }
    }
} 