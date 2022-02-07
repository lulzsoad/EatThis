using EatThisAPI.Database;
using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Models.ProposedRecipe;
using EatThisAPI.Models.ViewModels;
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
        Task<DataChunkViewModel<Recipe>> GetChunkOfVisibleRecipes(int skip, int take);
        Task<DataChunkViewModel<Recipe>> GetChunkOfVisibleRecipesByCategoryId(int categoryId, int skip, int take);
        Task<Recipe> GetRecipeById(int id);
        Task<int> AddProposedRecipe(
            ProposedRecipe proposedRecipe,
            ProposedCategory proposedCategory,
            List<ProposedIngredient> proposedIngredients,
            List<ProposedIngredientQuantity> proposedIngredientQuantities,
            List<ProposedStep> proposedSteps);
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

        public async Task<int> AddProposedRecipe(
            ProposedRecipe proposedRecipe, 
            List<ProposedIngredientQuantity> proposedIngredientQuantities, 
            List<ProposedIngredient> proposedIngredients,
            List<ProposedStep> proposedSteps, 
            ProposedCategory proposedCategory)
        {
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                }
            }

            return 1;
        }

        public async Task<DataChunkViewModel<Recipe>> GetChunkOfVisibleRecipes(int skip, int take)
        {
            var query = context.Recipes
                .AsNoTracking()
                .Where(x => x.IsVisible == true)
                .AsQueryable();

            var count = await query.CountAsync();
            var data = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new DataChunkViewModel<Recipe> {Data = data, Total = count };
        }

        public async Task<DataChunkViewModel<Recipe>> GetChunkOfVisibleRecipesByCategoryId(int categoryId, int skip, int take)
        {
            var query = context.Recipes
                .AsNoTracking()
                .Where(x => x.IsVisible == true && x.CategoryId == categoryId)
                .AsQueryable();

            var count = await query.CountAsync();
            var data = await query
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return new DataChunkViewModel<Recipe> { Data = data, Total = count };
        }

        public async Task<Recipe> GetRecipeById(int id)
        {
            return await context.Recipes
                .AsNoTracking()
                .Include(x => x.Steps)
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.IngredientQuantities)
                    .ThenInclude(x => x.Ingredient)
                    .ThenInclude(x => x.IngredientCategory)
                .Include(x => x.IngredientQuantities)
                    .ThenInclude(x => x.Unit)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> AddProposedRecipe(ProposedRecipe proposedRecipe, ProposedCategory proposedCategory, 
            List<ProposedIngredient> proposedIngredients, List<ProposedIngredientQuantity> proposedIngredientQuantities, 
            List<ProposedStep> proposedSteps)
        {
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.ProposedRecipes.Add(proposedRecipe);
                    await context.SaveChangesAsync();
                    if (proposedCategory != null)
                    {
                        proposedCategory.ProposedRecipeId = proposedRecipe.Id;
                        context.ProposedCategories.Add(proposedCategory);
                        await context.SaveChangesAsync();
                    }

                    context.ProposedIngredients.AddRange(proposedIngredients);
                    await context.SaveChangesAsync();

                    foreach (var proposedIngredientQuantity in proposedIngredientQuantities)
                    {
                        proposedIngredientQuantity.ProposedRecipeId = proposedRecipe.Id;
                        if (proposedIngredientQuantity.Reference != null)
                        {
                            proposedIngredientQuantity.ProposedIngredientId = proposedIngredients
                                .FirstOrDefault(x => x.Reference.Equals(proposedIngredientQuantity.Reference)).Id;
                        }
                    }
                    context.ProposedIngredientQuantities.AddRange(proposedIngredientQuantities);
                    await context.SaveChangesAsync();

                    foreach (var proposedStep in proposedSteps)
                    {
                        proposedStep.ProposedRecipeId = proposedRecipe.Id;
                    }

                    context.ProposedSteps.AddRange(proposedSteps);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(BackendMessage.Transaction.TRANSACTION_ERROR);
                }
            }

            return proposedRecipe.Id;
        }
    }
}
