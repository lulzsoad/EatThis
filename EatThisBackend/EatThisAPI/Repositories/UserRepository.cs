using EatThisAPI.Database;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Models.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IUserRepository
    {
        Task<int> RegisterUser(User newUser);
        Task<bool> EmailExists(string email);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(int id);
        Task<User> UpdateUser(User user, object updatedUser);
        Task ChangePassword(int userId, string newPassword);
        Task<DataChunkViewModel<User>> GetChunkOfUsers(int skip, int take, string search);
        Task ChangeUserRole(int userId, int roleId);
    }
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;
        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> RegisterUser(User newUser)
        {
            context.Users.Add(newUser);
            await context.SaveChangesAsync();
            return newUser.Id;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> GetUserById(int id)
        {
            return await context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> UpdateUser(User user, object updatedUser)
        {
            var userFromDb = context.Users.FirstOrDefault(x => x.Id == user.Id);
            if(userFromDb != null)
            {
                (updatedUser as JsonPatchDocument).ApplyTo(userFromDb);
                await context.SaveChangesAsync();
            }
            return userFromDb;
        }

        public async Task ChangePassword(int userId, string newPassword)
        {
            var userFromDb = context.Users.FirstOrDefault(x => x.Id == userId);
            userFromDb.PasswordHash = newPassword;
            await context.SaveChangesAsync();
        }

        public async Task<DataChunkViewModel<User>> GetChunkOfUsers(int skip, int take, string search)
        {
            var vm = new DataChunkViewModel<User>();
            var users = context.Users
                .AsNoTracking()
                .Include(x => x.Role)
                .OrderBy(x => x.RoleId)
                    .ThenBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .ThenBy(x=> x.Email)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(x =>
                    x.Email.Contains(search) ||
                    x.FirstName.Contains(search) ||
                    x.LastName.Contains(search) ||
                    x.Role.Name.Contains(search));
            }

            vm.Total = users.Count();
            vm.Data = await users.Skip(skip).Take(take)
                .Select(x => new User
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Role = x.Role,
                    IsActive = x.IsActive
                }).ToListAsync();

            return vm;
        }

        public async Task ChangeUserRole(int userId, int roleId)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            user.RoleId = roleId;
            await context.SaveChangesAsync();
        }
    }
}
