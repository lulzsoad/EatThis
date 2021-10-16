using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<Category> GetById(int id);
        Task<int> Add(Category category);
        Task Delete(Category category);
        Task<Category> Update(Category category);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;
        public CategoryRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Category>> GetAll() => await context.Categories.OrderBy(x => x.Name).ToListAsync();
        public async Task<Category> GetById(int id) => await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        public async Task<int> Add(Category category)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return category.Id;
        }
        public async Task Delete(Category category)
        {
            Category categoryToRemove = await context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            context.Remove(categoryToRemove);
            context.SaveChanges();
        }
        public async Task<Category> Update(Category category)
        {
            var categoryToEdit = await context.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            categoryToEdit.Name = category.Name;
            context.SaveChanges();
            return categoryToEdit;
        }
    }
}
