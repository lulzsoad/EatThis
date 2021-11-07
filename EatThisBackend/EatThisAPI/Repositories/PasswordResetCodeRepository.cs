using EatThisAPI.Database;
using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IPasswordResetCodeRepository
    {
        Task AddResetCode(string email, string code);
        Task Delete(string email, string code);
        Task<PasswordResetCode> CheckPasswordResetCode(PasswordResetCode passwordResetCode);
        Task<PasswordResetCode> GetPasswordResetCodeBySecuredRouteAndEmail(string securedRoute, string email);
        Task ChangePasswordByResetCode(PasswordResetCode passwordResetCode, User user);
    }
    public class PasswordResetCodeRepository : IPasswordResetCodeRepository
    {
        private readonly AppDbContext context;
        public PasswordResetCodeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddResetCode(string email, string code)
        {
            var passwordResetCode = context.PasswordResetCodes.FirstOrDefault(x => x.Email == email);
            if(passwordResetCode != null)
            {
                passwordResetCode.Code = code;
                context.PasswordResetCodes.Update(passwordResetCode);
                await context.SaveChangesAsync();
            }
            else
            {
                var model = new PasswordResetCode { Email = email, Code = code };
                context.PasswordResetCodes.Add(model);
                await context.SaveChangesAsync();
            }
        }

        public Task Delete(string email, string code)
        {
            throw new NotImplementedException();
        }

        public async Task<PasswordResetCode> CheckPasswordResetCode(PasswordResetCode passwordResetCode)
        {
            var prc = context.PasswordResetCodes.FirstOrDefault(x => x.Email == passwordResetCode.Email && x.Code == passwordResetCode.Code);
            if(prc != null)
            {
                prc.Code = null;
                prc.SecuredRoute = passwordResetCode.SecuredRoute;
                context.PasswordResetCodes.Update(prc);
                await context.SaveChangesAsync();
                return prc;
            }

            return null;
        }

        public async Task<PasswordResetCode> GetPasswordResetCodeBySecuredRouteAndEmail(string securedRoute, string email)
        {
            return await context.PasswordResetCodes.FirstOrDefaultAsync(x => x.SecuredRoute == securedRoute && x.Email == email);
        }

        public async Task ChangePasswordByResetCode(PasswordResetCode passwordResetCode, User user)
        {
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.PasswordResetCodes.Remove(passwordResetCode);
                    await context.SaveChangesAsync();

                    context.Users.Update(user);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(BackendMessage.Transaction.TRANSACTION_ERROR);
                }
                
            }
        }
    }
}
