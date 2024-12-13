using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleDataManagementSystem.Backend.Database.Entities;
using SimpleDataManagementSystem.Backend.Logic.DTOs;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.DTOs.Response;
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
    public class UsersCoreRepository : IUsersCoreRepository
    {
        private readonly SimpleDataManagementSystemDbContext _dbContext;
        private readonly IDbContextFactory<SimpleDataManagementSystemDbContext> _dbContextFactory;


        public UsersCoreRepository(
                SimpleDataManagementSystemDbContext dbContext,
                IDbContextFactory<SimpleDataManagementSystemDbContext> dbContextFactory
            )
        {
            _dbContext = dbContext;
            _dbContextFactory = dbContextFactory;
		}


        public async Task<User> CreateUserAsync(CreateUserRequestDTO createUserRequestDTO, CancellationToken cancellationToken)
        {
            var newUserEntity = new UserEntity()
            {
                CreatedUTC = DateTime.UtcNow,
                RoleId = (int)createUserRequestDTO.Role,
                Username = createUserRequestDTO.Username
            };

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();
            newUserEntity.PasswordHash = passwordHasher.HashPassword(newUserEntity, createUserRequestDTO.Password);

            var createdUserEntity = await _dbContext.Users.AddAsync(newUserEntity, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var newUser = new User()
            {
                ID = createdUserEntity.Entity.Id,
                IsPasswordChangeRequired = createdUserEntity.Entity.IsPasswordChangeRequired,
                Role = new Role()
                {
                    ID = (int)createdUserEntity.Entity.RoleId
                },
                Username = newUserEntity.Username
            };

            return newUser;
        }

        public async Task DeleteUserAsync(DeleteUserRequestDTO deleteUserRequestDTO, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.FindAsync(deleteUserRequestDTO.RequestedByUserId, cancellationToken);

            if (existingUser == null)
            {
                return;
            }

            _dbContext.Users.Remove(existingUser);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<User?> GetUserAsync(GetUserRequestDTO getUserRequestDTO, CancellationToken cancellationToken)
        {
            using (var db = await _dbContextFactory.CreateDbContextAsync(cancellationToken))
            {
                var user = await db.Users
                                        .Where(x => x.Id == getUserRequestDTO.UserId)
                                        .Include(x => x.Role)
                                        .SingleOrDefaultAsync(cancellationToken);
                //var user = await db.Users.FindAsync(getUserRequestDTO.UserId, cancellationToken);

                if (user == null)
                {
                    return null;
                }

                var response = new User()
                {
                    ID = user.Id,
                    IsPasswordChangeRequired = user.IsPasswordChangeRequired,
                    Role = new Role()
                    {
                        ID = user.Role.Id,
                        Name = user.Role.Name
                    },
                    Username = user.Username
                };

                return response;
            }
        }

        public async Task<User?> GetUserByLoginCredentialsAsync(
                GetUserByLoginCredentialsRequestDTO getUserByLoginCredentialsRequestDTO, 
                CancellationToken cancellationToken
            )
        {
            if (
                    string.IsNullOrEmpty(getUserByLoginCredentialsRequestDTO.Username) 
                    || 
                    string.IsNullOrEmpty(getUserByLoginCredentialsRequestDTO.Password)
                )
            {
                return null;
            }

            var userEntity = await _dbContext.Users
                .Include(x => x.Role)
                .Where(x => x.Username == getUserByLoginCredentialsRequestDTO.Username)
                .FirstOrDefaultAsync(cancellationToken);

            if (userEntity == null)
            {
                return null;
            }

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();
            var match = passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, getUserByLoginCredentialsRequestDTO.Password);

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

        public async Task<UsersDTO> GetUsersAsync(GetUsersRequestDTO getUsersRequestDTO, CancellationToken cancellationToken)
        {
            using var usersDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            using var countDbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

            var usersQueryable = usersDbContext
                                            .Users
                                            .Include(x => x.Role)
				                            .Skip((getUsersRequestDTO.PageInfo.Page - 1) * getUsersRequestDTO.PageInfo.Take)
				                            .Take(getUsersRequestDTO.PageInfo.Take)
				                            .OrderBy(x => x.Username);

            var countQueryable = countDbContext.Users;

			//var queryable = _dbContext.Users.Include(x => x.Role);

            //var usersQueryable = queryable
            //                        .Skip((getUsersRequestDTO.PageInfo.Page - 1) * getUsersRequestDTO.PageInfo.Take)
            //                        .Take(getUsersRequestDTO.PageInfo.Take)
            //                        .OrderBy(x => x.Username);

            //var countQueryable = queryable;

            var countTask = countQueryable.CountAsync(cancellationToken);
            var usersTask = usersQueryable.ToListAsync(cancellationToken);

            await Task.WhenAll(countTask, usersTask);

            var countTaskResult = await countTask;
            var usersTaskResult = await usersTask;

            var response = new UsersDTO()
            {
                PageInfo = new UsersDTO.PageDTO()
                {
                    Total = countTaskResult
                },
                Users = usersTaskResult.Select
                (
                    x => new User()
                    {
                        ID = x.Id,
                        IsPasswordChangeRequired = x.IsPasswordChangeRequired,
                        Role = new Role()
                        {
                            ID = x.Role.Id,
                            Name = x.Role.Name
                        },
                        Username = x.Username
                    }
                ).ToList()
            };

            return response;
        }

        public async Task UpdatePasswordAsync(UpdatePasswordRequestDTO updatePasswordRequestDTO, CancellationToken cancellationToken)
        {
            if (updatePasswordRequestDTO == null)
            {
                return;
            }

            var userEntity = await _dbContext
                .Users
                .Where(x => x.Id == updatePasswordRequestDTO.RequestMetadata.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (userEntity == null)
            {
                return;
            }

            PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();
            var match = passwordHasher.VerifyHashedPassword(userEntity, userEntity.PasswordHash, updatePasswordRequestDTO.OldPassword);

            if (match == PasswordVerificationResult.Failed)
            {
                throw new NotAllowedException("Invalid password.");
            }

            userEntity.PasswordHash = passwordHasher.HashPassword(userEntity, updatePasswordRequestDTO .NewPassword);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateUserAsync(UpdateUserRequestDTO updateUserRequestDTO, CancellationToken cancellationToken)
        {
            var existingUser = await _dbContext.Users.FindAsync(updateUserRequestDTO.RequestMetadata.UserId);
            
            if (existingUser == null)
            {
                return;
            }

            existingUser.Username = updateUserRequestDTO.Username;
            existingUser.Role = new RoleEntity()
            {
                Id = (int)updateUserRequestDTO.Role
            };

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
