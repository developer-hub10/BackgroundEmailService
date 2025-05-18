using BackgroundEmailService.Models;
using BackgroundEmailService.Data;
using BackgroundEmailService.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BackgroundEmailService.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _config;

        public AuthRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task Register(Auth data)
        {
            if (await _context.Auths.AnyAsync(a => a.Username == data.Username))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            // Hash the password
            data.Password = new PasswordHasher<Auth>().HashPassword(data, data.Password);

            // Add the new user
            await _context.Auths.AddAsync(data);
            await _context.SaveChangesAsync();
        }


        public async Task<string> Login(string username, string password)
        {
            var credentials = await _context.Auths.FirstOrDefaultAsync(a => a.Username == username);
            if (credentials == null) return null;

            var result = new PasswordHasher<Auth>()
                .VerifyHashedPassword(credentials, credentials.Password, password);

            if (result == PasswordVerificationResult.Failed) return null;

            return new JwtService(_config).GenerateToken(credentials.Id, credentials.Username, credentials.Role);
        }

    }
}
