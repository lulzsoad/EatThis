using EatThisAPI.Database;
using EatThisAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IPasswordResetCodeRepository
    {
        Task Add(string email, string code, string securedRoute);
        Task Delete(string email, string code);
    }
    public class PasswordResetCodeRepository : IPasswordResetCodeRepository
    {
        private readonly AppDbContext context;
        public PasswordResetCodeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task Add(string email, string code, string securedRoute)
        {
            var model = new PasswordResetCode { Email = email, Code = code };
            context.PasswordResetCodes.Add(model);
            await context.SaveChangesAsync(); 
        }

        public Task Delete(string email, string code)
        {
            throw new NotImplementedException();
        }
    }
}
