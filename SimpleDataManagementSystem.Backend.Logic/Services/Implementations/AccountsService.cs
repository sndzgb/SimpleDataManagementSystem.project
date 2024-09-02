using SimpleDataManagementSystem.Backend.Logic.Constants;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class AccountsService : IAccountsService
    {
        private readonly IAccountsRepository _accountsRepository;


        public AccountsService(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }


        public async Task<UserLogInResultDTO?> LogInAsync(string username, string password)
        {
            var user = await _accountsRepository.LogInAsync(username, password);

            if (user == null) 
            {
                return null;
            }

            //if (user.Claims.Where(x => x.Key == "Role").FirstOrDefault().Value == Roles.User.ToString()) // "User" role not allowed to login
            //{
            //    return null;
            //}

            return user;
        }
    }
}
