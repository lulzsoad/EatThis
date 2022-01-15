using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IIngredientQuantityRepository
    {
        Task<List<IngredientQuantity>> GetIngretientQuantitiesByRecipeId(int recipeId);
    }
    public class IngredientQuantityRepository : IIngredientQuantityRepository
    {
        private AppDbContext context;
        public IngredientQuantityRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<List<IngredientQuantity>> GetIngretientQuantitiesByRecipeId(int recipeId)
        {
            return await context.IngredientQuantities
                .Where(x => x.RecipeId == recipeId)
                .ToListAsync();
        }
    }
}
