using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetRoleById(int id);
    }

    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext dbContext;
        public RoleRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await dbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
