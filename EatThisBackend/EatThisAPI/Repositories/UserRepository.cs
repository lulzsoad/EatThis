using EatThisAPI.Database;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs.User;
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
    }
}
