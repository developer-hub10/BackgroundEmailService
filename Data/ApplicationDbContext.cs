using Microsoft.EntityFrameworkCore;
using BackgroundEmailService.Models;

namespace BackgroundEmailService.Data
{


    public class ApplicationDbContext : DbContext
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }




        public DbSet<Email> Emails { get; set; }

        public DbSet<Applicant> Applicants { get; set; }

        public DbSet<Auth> Auths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Email>(entity =>
            {
                entity.Property(e => e.status)
                      .HasDefaultValue("pending");

                entity.Property(e => e.times)
                      .HasDefaultValue(0);
            });

            modelBuilder.Entity<Applicant>(entity =>
             {
                 entity.Property(e => e.Email)
                       .IsRequired();

                 entity.HasIndex(e => e.Email)
                       .IsUnique();
             });

            base.OnModelCreating(modelBuilder);
        }


    }





}