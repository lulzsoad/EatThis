using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IStepRepository
    {
        Task<List<Step>> GetStepsByRecipeId(int recipeId);
    }
    public class StepRepository : IStepRepository
    {
        private AppDbContext context;
        public StepRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Step>> GetStepsByRecipeId(int recipeId)
        {
            return await context.Steps
                .Where(x => x.RecipeId == recipeId)
                .OrderBy(x => x.Order)
                .ToListAsync();
        }
    }
}
