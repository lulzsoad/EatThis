using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IAccountRepository
    {
        Task RegisterUser(User newUser);
        Task<bool> EmailExists(string email);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext context;
        public AccountRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task RegisterUser(User newUser)
        {
            context.Users.Add(newUser);
            await context.SaveChangesAsync();
        }

        public async Task<bool> EmailExists(string email)
        {
            return await context.Users.AnyAsync(x => x.Email == email);
        }
    }
}
