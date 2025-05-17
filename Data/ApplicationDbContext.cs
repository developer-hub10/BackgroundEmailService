using Microsoft.EntityFrameworkCore;


namespace BackgroundEmailService.Data
{


    public class ApplicationDbContext : DbContext
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        
    }
}