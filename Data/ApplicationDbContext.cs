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

    }
    

    

   
}