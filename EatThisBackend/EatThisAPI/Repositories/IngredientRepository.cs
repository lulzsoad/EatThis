using EatThisAPI.Database;
using EatThisAPI.Models;
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
    }

    public class IngredientRepository : IIngredientRepository
    {
        private readonly AppDbContext context;
        public IngredientRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Ingredient>> GetAll() => await context.Ingredients.OrderBy(x => x.Name).ToListAsync();
        public async Task<Ingredient> GetById(int id) => await context.Ingredients.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<int> Add(Ingredient ingredient)
        {
            context.Ingredients.Add(ingredient);
            await context.SaveChangesAsync();
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
            context.SaveChanges();
            return ingredientToEdit;
        }
    }
}
