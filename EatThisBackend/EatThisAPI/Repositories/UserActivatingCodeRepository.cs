using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IUserActivatingCodeRepository
    {
        Task Add(UserActivatingCode userActivatingCode);
        Task<bool> CheckAndActivateAccount(string activationCode);
    }
    public class UserActivatingCodeRepository : IUserActivatingCodeRepository
    {
        private readonly AppDbContext context;
        private readonly IUserRepository userRepository;
        public UserActivatingCodeRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task Add(UserActivatingCode userActivatingCode)
        {
            await context.UserActivatingCodes.AddAsync(userActivatingCode);
            await context.SaveChangesAsync();
        }

        public async Task<bool> CheckAndActivateAccount(string activationCode)
        {
            var userActivatingCode = context.UserActivatingCodes.FirstOrDefault(x => x.ActivatingCode == activationCode);
            if(userActivatingCode == null)
            {
                return false;
            }

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userActivatingCode.UserId);
                    user.IsActive = true;
                    await context.SaveChangesAsync();
                    context.Remove(userActivatingCode);
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Wystąpił błąd podczas aktywacji konta");
                }
            }

            return true;
        }
    }
}
