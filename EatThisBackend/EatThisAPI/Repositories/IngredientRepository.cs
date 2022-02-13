using EatThisAPI.Database;
using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Models.ProposedRecipe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetAll();
        Task<Ingredient> GetById(int id);
        Task<int> Add(Ingredient ingredient);
        Task Delete(Ingredient ingredient);
        Task<Ingredient> Update(Ingredient ingredient);
        Task<bool> CheckIfExists(Ingredient ingredient);
        Task<bool> CheckIfExistsById(int id);
        Task<ProposedIngredient> GetProposedIngredientById(int id);
        Task<bool> CheckIfExists(ProposedIngredient ingredient);
        Task<Ingredient> AcceptProposedIngredient(ProposedIngredient proposedIngredient);
        Task<ProposedIngredientQuantity> GetProposedIngredientQuantityByReference(string reference);
        Task<List<ProposedIngredientQuantity>> GetProposedIngredientQuantitiesByProposedRecipeId(int proposedRecipeId);
        Task DeleteProposedIngredient(int id);
    }

    public class IngredientRepository : IIngredientRepository
    {
        private readonly AppDbContext context;
        public IngredientRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAll() => await context.Ingredients.Include(x => x.IngredientCategory).OrderBy(x => x.Name).ToListAsync();
        public async Task<Ingredient> GetById(int id) => await context.Ingredients.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<int> Add(Ingredient ingredient)
        {
            context.Ingredients.Add(ingredient);
            try
            {
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
            
            return ingredient.Id;
        }
        public async Task Delete(Ingredient ingredient)
        {
            Ingredient ingredientToRemove = await context.Ingredients.FirstOrDefaultAsync(x => x.Id == ingredient.Id);
            context.Remove(ingredientToRemove);
            context.SaveChanges();
        }
        public async Task<Ingredient> Update(Ingredient ingredient)
        {
            var ingredientToEdit = await context.Ingredients.FirstOrDefaultAsync(x => x.Id == ingredient.Id);
            ingredientToEdit.Name = ingredient.Name;
            ingredientToEdit.IngredientCategoryId = ingredient.IngredientCategoryId;
            context.SaveChanges();
            return ingredientToEdit;
        }

        public async Task<bool> CheckIfExists(Ingredient ingredient) => await context.Ingredients.AnyAsync(x => x.Name == ingredient.Name);
        public async Task<bool> CheckIfExists(ProposedIngredient ingredient) => await context.ProposedIngredients.AnyAsync(x => x.Id == ingredient.Id);
        public async Task<bool> CheckIfExistsById(int id) => await context.Ingredients.AnyAsync(x => x.Id == id);

        public async Task<ProposedIngredient> GetProposedIngredientById(int id) => 
            await context.ProposedIngredients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Ingredient> AcceptProposedIngredient(ProposedIngredient proposedIngredient)
        {
            var ingredient = new Ingredient();
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    ingredient.Name = proposedIngredient.Name;
                    ingredient.IngredientCategoryId = proposedIngredient.IngredientCategoryId;
                    context.Ingredients.Add(ingredient);
                    await context.SaveChangesAsync();

                    var proposedIngredientQuantity = await context.ProposedIngredientQuantities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Reference == proposedIngredient.Reference);

                    proposedIngredientQuantity.ProposedIngredient = null;
                    proposedIngredientQuantity.ProposedIngredientId = null;
                    proposedIngredientQuantity.IngredientId = ingredient.Id;
                    context.ProposedIngredientQuantities.Update(proposedIngredientQuantity);
                    await context.SaveChangesAsync();

                    context.ProposedIngredients.Remove(proposedIngredient);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(BackendMessage.Transaction.TRANSACTION_ERROR);
                }
            }

            return ingredient;
        }

        public async Task<ProposedIngredientQuantity> GetProposedIngredientQuantityByReference(string reference)
        {
            return context.ProposedIngredientQuantities
                        .Include(x => x.Unit)
                        .Include(x => x.Ingredient)
                        .ThenInclude(x => x.IngredientCategory)
                        .FirstOrDefault(x => x.Reference == reference);
        }

        public async Task<List<ProposedIngredientQuantity>> GetProposedIngredientQuantitiesByProposedRecipeId(int proposedRecipeId)
        {
            return await context.ProposedIngredientQuantities
                .AsNoTracking()
                .Include(x => x.Ingredient)
                    .ThenInclude(x => x.IngredientCategory)
                .Include(x => x.ProposedIngredient)
                    .ThenInclude(x => x.IngredientCategory)
                .Include(x => x.Unit)
                .Where(x => x.ProposedRecipeId == proposedRecipeId)
                .ToListAsync();
        }

        public async Task DeleteProposedIngredient(int id)
        {
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var proposedIngredient = context.ProposedIngredients.AsNoTracking().FirstOrDefault(x => x.Id == id);
                    var proposedIngredientQuantity = context.ProposedIngredientQuantities
                        .AsNoTracking().FirstOrDefault(x => x.ProposedIngredientId == proposedIngredient.Id);

                    context.ProposedIngredientQuantities.Remove(proposedIngredientQuantity);
                    await context.SaveChangesAsync();

                    context.ProposedIngredients.Remove(proposedIngredient);
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
