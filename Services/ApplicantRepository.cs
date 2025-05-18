using BackgroundEmailService.Repository;
using BackgroundEmailService.Models;
using BackgroundEmailService.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackgroundEmailService.Services
{
    
  
    public class ApplicantRepository : IApplicantRepository
    {
         
         private readonly ApplicationDbContext _context;

         public ApplicantRepository(ApplicationDbContext context) 
         {
             _context = context;
         }


          public async Task<int> RegisterApplicantAsync(Applicant data)
          {
                await _context.Applicants.AddAsync(data);
                await _context.SaveChangesAsync();
                return data.Id;
          }

            public async Task<List<Applicant>> GetAllApplicant()
            {
                var result = await _context.Applicants.ToListAsync();
                return result;
            }
    

    }

}