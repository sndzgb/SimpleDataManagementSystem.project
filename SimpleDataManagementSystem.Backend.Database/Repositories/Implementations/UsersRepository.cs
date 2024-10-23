using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Read;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Write;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.Logic.Models;
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
            if (newUserDTO == null)
            {
                throw new ArgumentNullException(nameof(newUserDTO));
            }

            var newUserEntity = new UserEntity()
            {
                CreatedUTC = DateTime.UtcNow,
                RoleId = newUserDTO.RoleId,
                Username = newUserDTO.Username
            };

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();
            newUserEntity.PasswordHash = passwordHasher.HashPassword(newUserEntity, newUserDTO.Password);

            var newUserId = await _dbContext.Users.AddAsync(newUserEntity);

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

        public async Task<UsersDTO?> GetAllUsersAsync(int? take = 8, int? page = 1)
        {
            var total = await _dbContext.Users.CountAsync();

            var users = new UsersDTO();

            users.PageInfo.Total = total;
            users.PageInfo.Take = (int)take!;
            users.PageInfo.Page = (int)page!;

            if (total > 0)
            {
                users.Users.AddRange
                (
                    await _dbContext.Users
                        .OrderBy(x => x.Id)
                        .Skip((page!.Value - 1) * take!.Value)
                        .Take(take.Value)
                        .Include(i => i.Role)
                        .Select(s => new UserDTO()
                        {
                            ID = s.Id,
                            Username = s.Username,
                            RoleId = s.Role == null ? null : s.Role.Id,
                            RoleName = s.Role == null ? null : s.Role.Name,
                            IsPasswordChangeRequired = s.IsPasswordChangeRequired
                        })
                        .ToListAsync()
                );
            }

            return users;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var userEntity = await _dbContext.Users.Where(x => x.Id == userId).Include(x => x.Role).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                return null;
            }

            var user = new User()
            {
                ID = userId,
                Username = userEntity.Username,
                IsPasswordChangeRequired = userEntity.IsPasswordChangeRequired,
                PasswordHash = userEntity.PasswordHash,
            };

            if (userEntity.Role != null) 
            {
                user.Role = new Role()
                {
                    ID = userEntity.Role!.Id,
                    Name = userEntity.Role!.Name
                };
        }

            return user;
        }

        public async Task<User?> GetUserByLogInCredentialsAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var userEntity = await _dbContext.Users
                .Include(x => x.Role)
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();

            if (userEntity == null)
            {
                return null;
            }

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();
            var match = passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, password);

            if (match == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var user = new User()
            {
                ID = userEntity.Id,
                PasswordHash = userEntity.PasswordHash,
                Username = userEntity.Username,
                IsPasswordChangeRequired = userEntity.IsPasswordChangeRequired
            };

            if (userEntity.Role != null)
            {
                user.Role = new Role()
                {
                    ID = userEntity.Role.Id,
                    Name = userEntity.Role.Name
                };
            }

            return user;
        }

        public async Task UpdatePasswordAsync(int userId, UpdatePasswordDTO updatePasswordDTO)
        {
            if (updatePasswordDTO == null)
            {
                return;
            }

            var userEntity = await _dbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            if (userEntity == null)
            {
                throw new RequiredRecordNotFoundException("User was not found.");
            }

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();
            var match = passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, updatePasswordDTO.OldPassword);

            if (match == PasswordVerificationResult.Failed)
            {
                throw new NotAllowedException("Invalid password.");
            }

            userEntity.PasswordHash = passwordHasher.HashPassword(userEntity, updatePasswordDTO.NewPassword);
            //userEntity.IsPasswordChangeRequired = false;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO)
        {
            if (updateUserDTO == null)
            {
                return;
            }

            var user = await _dbContext.Users
                .Where(x => x.Id == userId)
                .Include(x => x.Role)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return;
            }

            user.Username = updateUserDTO.Username;
            user.RoleId = updateUserDTO.RoleId;
            user.IsPasswordChangeRequired = updateUserDTO.IsPasswordChangeRequired;

            await _dbContext.SaveChangesAsync();
        }
    }
}
