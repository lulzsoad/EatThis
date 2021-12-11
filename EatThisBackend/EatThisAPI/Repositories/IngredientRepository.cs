﻿using EatThisAPI.Database;
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
        Task<bool> CheckIfExists(Ingredient ingredient);
        Task<bool> CheckIfExistsById(int id);
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
        public async Task<bool> CheckIfExistsById(int id) => await context.Ingredients.AnyAsync(x => x.Id == id);
    }
}
