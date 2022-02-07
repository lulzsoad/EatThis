using EatThisAPI.Models;
using EatThisAPI.Models.ProposedRecipe;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientQuantity> IngredientQuantities { get; set; }
        public DbSet<IngredientCategory> IngredientCategories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserActivatingCode> UserActivatingCodes { get; set; }
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }
        public DbSet<ProposedRecipe> ProposedRecipes { get; set; }
        public DbSet<ProposedIngredient> ProposedIngredients { get; set; }
        public DbSet<ProposedIngredientQuantity> ProposedIngredientQuantities { get; set; }
        public DbSet<ProposedCategory> ProposedCategories { get; set; }
        public DbSet<ProposedStep> ProposedSteps { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProposedIngredientQuantity>()
                .Property(x => x.ProposedRecipeId)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Category>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Ingredient>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(75);
            modelBuilder.Entity<Ingredient>()
                .Property(x => x.IngredientCategoryId)
                .IsRequired();
            modelBuilder.Entity<Ingredient>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<IngredientQuantity>()
                .Property(x => x.IngredientId)
                .IsRequired();
            modelBuilder.Entity<IngredientQuantity>()
                .Property(x => x.RecipeId)
                .IsRequired();

            modelBuilder.Entity<IngredientCategory>()
                .Property(x => x.Name)
                .IsRequired();
            modelBuilder.Entity<IngredientCategory>()
                .Property(x => x.Name)
                .HasMaxLength(100);
            modelBuilder.Entity<IngredientCategory>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Recipe>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(75);
            modelBuilder.Entity<Recipe>()
                .Property(x => x.Name)
                .HasMaxLength(75);
            modelBuilder.Entity<Recipe>()
                .Property(x => x.IsVisible)
                .IsRequired();
            modelBuilder.Entity<Recipe>()
                .Property(x => x.Difficulty)
                .IsRequired();

            modelBuilder.Entity<Step>()
                .Property(x => x.Description)
                .IsRequired();
            modelBuilder.Entity<Step>()
                .Property(x => x.RecipeId)
                .IsRequired();

            modelBuilder.Entity<Unit>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30);
            modelBuilder.Entity<Unit>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .Property(x => x.Name)
                .IsRequired();
        }
    }
}
