using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;


        public UsersRepository(SimpleDataManagementSystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<int> AddNewUserAsync(NewUserDTO newUserDTO)
        {
            var newUserId = await _dbContext.Users.AddAsync(new UserEntity()
            {
                Username = newUserDTO.Username,
                Password = newUserDTO.Password,
                RoleId = newUserDTO.RoleId
            });

            await _dbContext.SaveChangesAsync();

            return newUserId.Entity.Id;
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = _dbContext.Users.Find(userId);

            if (user == null)
            {
                return;
            }

            _dbContext.Remove(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserDTO>> GetAllUsersAsync(int? take = 8, int? page = 1)
        {
            return await _dbContext.Users.OrderBy(x => x.Id)
                .Skip((page!.Value - 1) * take!.Value)
                .Take(take.Value)
                .Include(i => i.Role)
                .Select(s => new UserDTO()
                {
                    ID = s.Id,
                    RoleName = s.Role.Name,
                    Username = s.Username
                })
                .ToListAsync();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int userId)
        {
            var userEntity = await _dbContext.Users.Where(x => x.Id == userId).Include(x => x.Role).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                return null;
            }

            var userDTO = new UserDTO()
            {
                ID = userId,
                RoleName = userEntity.Role.Name,
                Username = userEntity.Username,
                RoleId = userEntity.Role.Id
            };

            return userDTO;
        }

        public async Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO)
        {
            var user = await _dbContext.Users
                .Where(x => x.Id == userId)
                .Include(x => x.Role)
                .FirstOrDefaultAsync();

            if (user == null) 
            {
                return;
            }

            user.RoleId = updateUserDTO.RoleId;
            user.Username = updateUserDTO.Username;
            
            await _dbContext.SaveChangesAsync();
        }
    }
}
