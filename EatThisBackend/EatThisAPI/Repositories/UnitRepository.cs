using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Unit>> GetAll();
        Task<Unit> GetById(int id);
        Task<int> Add(Unit unit);
        Task Delete(Unit unit);
        Task<Unit> Update(Unit unit);
    }

    public class UnitRepository : IUnitRepository
    {
        private readonly AppDbContext context;
        public UnitRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Unit>> GetAll() => await context.Units.OrderBy(x => x.Name).ToListAsync();
        public async Task<Unit> GetById(int id) => await context.Units.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<int> Add(Unit unit)
        {
            context.Units.Add(unit);
            await context.SaveChangesAsync();
            return unit.Id;
        }
        public async Task Delete(Unit unit)
        {
            Unit unitToRemove = await context.Units.FirstOrDefaultAsync(x => x.Id == unit.Id);
            context.Remove(unitToRemove);
            context.SaveChanges();
        }
        public async Task<Unit> Update(Unit unit)
        {
            var unitToEdit = await context.Units.FirstOrDefaultAsync(x => x.Id == unit.Id);
            unitToEdit.Name = unit.Name;
            context.SaveChanges();
            return unitToEdit;
        }
    }
}
