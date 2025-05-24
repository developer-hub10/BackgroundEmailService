using Microsoft.EntityFrameworkCore;
using BackgroundEmailService.Repository;
using BackgroundEmailService.Models;
using BackgroundEmailService.Data;

using Dapper;
using MySqlConnector;
using System.Text;
using System.Linq;

namespace BackgroundEmailService.Services
{


    public class EmailRepository : IEmailRepository
    {

        private readonly ApplicationDbContext _context;



        private readonly IConfiguration _config;
        public EmailRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<List<Email>> GetAllEmails()
        {
            return await _context.Emails.ToListAsync();
        }

        public async Task<List<Email>> GetPendingEmails()
        {
            string connectionString = _config.GetConnectionString("MySqlConnection");
            await using (var conn = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT email FROM Emails WHERE status = 'pending'";
                var result = await conn.QueryAsync<Email>(query);
                return result.ToList();
            }
        }

        public async Task<int> RegisterEmails(List<Email> emailList)
        {
            string connectionString = _config.GetConnectionString("MySqlConnection");
            await using var conn = new MySqlConnection(connectionString);
            int batchSize = 100;
            int rowsAff = 0;

            for (int start = 0; start < emailList.Count; start += batchSize)
            {
                var batch = emailList.Skip(start).Take(batchSize).ToList();
                var insertQuery = new StringBuilder("INSERT INTO Emails (email) VALUES ");

                var parameters = new DynamicParameters();

                for (int index = 0; index < batch.Count; index++)
                {
                    insertQuery.Append($"(@Email{index}),");  // Note the closing parenthesis and comma
                    parameters.Add($"Email{index}", batch[index].UserEmail);
                }

                insertQuery.Length--; // Remove the last comma

                rowsAff += await conn.ExecuteAsync(insertQuery.ToString(), parameters); // Use += to accumulate affected rows
            }

            return rowsAff;
        }

        // public Task RegisterEmail(Email email);

        // public Task UpdateEmail(Email email);

        // public Task DeleteEmail(int id);
    }


}