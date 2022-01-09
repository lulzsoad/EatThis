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
    public interface IRecipeRepository
    {
        Task<int> AddRecipe(Recipe recipe, List<IngredientQuantity> ingredientQuantities, List<Step> steps);
        Task<List<Recipe>> GetAllVisibleRecipes(int skip, int take);
        Task<List<Recipe>> GetAllVisibleRecipesByCategoryId(int categoryId, int skip, int take);
    }
    public class RecipeRepository : IRecipeRepository
    {
        private AppDbContext context;
        public RecipeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddRecipe(Recipe recipe, List<IngredientQuantity> ingredientQuantities, List<Step> steps)
        {
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.Recipes.Add(recipe);
                    await context.SaveChangesAsync();

                    for (int i = 0; i < ingredientQuantities.Count; i++)
                    {
                        ingredientQuantities.ElementAt(i).RecipeId = recipe.Id;
                    }

                    for (int i = 0; i < steps.Count; i++)
                    {
                        steps.ElementAt(i).RecipeId = recipe.Id;
                    }

                    context.IngredientQuantities.AddRange(ingredientQuantities);
                    context.Steps.AddRange(steps);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(BackendMessage.Transaction.TRANSACTION_ERROR);
                }
            }

            return recipe.Id;
        }

        public async Task<List<Recipe>> GetAllVisibleRecipes(int skip, int take)
        {
            return await context.Recipes
                .Skip(skip)
                .Take(take)
                .Where(x => x.IsVisible == true)
                .ToListAsync();
        }

        public async Task<List<Recipe>> GetAllVisibleRecipesByCategoryId(int categoryId, int skip, int take)
        {
            return await context.Recipes
                .Skip(skip)
                .Take(take)
                .Where(x => x.IsVisible == true && x.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
