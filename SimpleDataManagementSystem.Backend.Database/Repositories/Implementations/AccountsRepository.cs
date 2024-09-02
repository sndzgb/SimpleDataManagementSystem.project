using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Repositories.Implementations
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public AccountsRepository(SimpleDataManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<UserLogInResultDTO?> LogInAsync(string username, string password)
        {
            var user = await _dbContext.Users
                .Where(x => x.Username == username && x.Password == password)
                .Include(x => x.Role).FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }

            var dto = new UserLogInResultDTO()
            {
                Claims = new List<UserLogInResultDTO.MyClaim>()
                {
                    new UserLogInResultDTO.MyClaim()
                    {
                        Key = "Role",
                        Value = user.Role.Name
                    }
                },
                Username = user.Username
            };

            return dto;
        }
    }
}
