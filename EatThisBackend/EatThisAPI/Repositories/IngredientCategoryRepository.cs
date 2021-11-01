using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IIngredientCategoryRepository
    {
        Task<IEnumerable<IngredientCategory>> GetAll();
        Task<IngredientCategory> GetById(int id);
        Task<int> Add(IngredientCategory ingredientCategory);
        Task Delete(IngredientCategory ingredientCategory);
        Task<IngredientCategory> Update(IngredientCategory ingredientCategory);
        Task<bool> CheckIfExists(IngredientCategory ingredientCategory);
        Task<bool> CheckIfExistsById(int id);
    }

    public class IngredientCategoryRepository : IIngredientCategoryRepository
    {
        private readonly AppDbContext context;
        public IngredientCategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<IngredientCategory>> GetAll() => await context.IngredientCategories.OrderBy(x => x.Name).ToListAsync();
        public async Task<IngredientCategory> GetById(int id) => await context.IngredientCategories.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<int> Add(IngredientCategory ingredientCategory)
        {
            context.IngredientCategories.Add(ingredientCategory);
            await context.SaveChangesAsync();
            return ingredientCategory.Id;
        }
        public async Task Delete(IngredientCategory ingredientCategory)
        {
            IngredientCategory ingredientCategoryToRemove = await context.IngredientCategories.FirstOrDefaultAsync(x => x.Id == ingredientCategory.Id);
            context.Remove(ingredientCategoryToRemove);
            context.SaveChanges();
        }
        public async Task<IngredientCategory> Update(IngredientCategory ingredientCategory)
        {
            var ingredientCategoryToEdit = await context.IngredientCategories.FirstOrDefaultAsync(x => x.Id == ingredientCategory.Id);
            ingredientCategoryToEdit.Name = ingredientCategory.Name;
            context.SaveChanges();
            return ingredientCategoryToEdit;
        }
        public async Task<bool> CheckIfExists(IngredientCategory ingredientCategory) => await context.IngredientCategories.AnyAsync(x => x.Name == ingredientCategory.Name);
        public async Task<bool> CheckIfExistsById(int id) => await context.IngredientCategories.AnyAsync(x => x.Id == id);
    }
}
