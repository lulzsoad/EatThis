﻿using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.ViewModels;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IRecipeService
    {
        Task<int> AddRecipe(RecipeDto recipeDto);
        Task<DataChunkViewModel<RecipeDto>> GetChunkOfRecipesByCategory(string categoryId, int skip, int take);
    }
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IValidator validator;
        private readonly IUserHelper userHelper;
        public RecipeService(
            IRecipeRepository recipeRepository,
            IValidator validator,
            IUserHelper userHelper
            )
        {
            this.recipeRepository = recipeRepository;
            this.validator = validator;
            this.userHelper = userHelper;
        }

        public async Task<int> AddRecipe(RecipeDto recipeDto)
        {
            validator.IsObjectNull(recipeDto);
            var user = await this.userHelper.GetCurrentUser();
            var recipe = new Recipe()
            {
                Name = recipeDto.Name,
                SubName = recipeDto.SubName,
                Description = recipeDto.Description,
                Difficulty = recipeDto.Difficulty,
                IsVisible = recipeDto.IsVisible,
                CategoryId = recipeDto.Category.Id,
                Image = recipeDto.Image,
                Time = recipeDto.Time,
                PersonQuantity = recipeDto.PersonQuantity,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow
            };

            var ingredientQuantities = new List<IngredientQuantity>();
            foreach(var item in recipeDto.IngredientQuantities)
            {
                ingredientQuantities.Add(new IngredientQuantity
                {
                    IngredientId = item.Ingredient.Id,
                    Description = item.Description,
                    UnitId = item.Unit.Id,
                    Quantity = item.Quantity
                });
            }

            var steps = new List<Step>();
            foreach(var step in recipeDto.Steps)
            {
                steps.Add(new Step
                {
                    Description = step.Description,
                    Image = step.Image,
                    Order = step.Order
                });
            }

            int id = await recipeRepository.AddRecipe(recipe, ingredientQuantities, steps);
            return id;
        }

        public async Task<DataChunkViewModel<RecipeDto>> GetChunkOfRecipesByCategory(string categoryId, int skip, int take)
        {
            var vm = new DataChunkViewModel<Recipe>();
            List<RecipeDto> recipeDtos = new List<RecipeDto>();

            if (string.IsNullOrEmpty(categoryId) || categoryId == "0")
            {
                vm = await recipeRepository.GetChunkOfVisibleRecipes(skip, take);
            }
            else
            {
                int id = Convert.ToInt32(categoryId);
                vm = await recipeRepository.GetChunkOfVisibleRecipesByCategoryId(id, skip, take);
            }

            foreach(var recipe in vm.Data)
            {
                recipeDtos.Add(new RecipeDto
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    SubName = recipe.SubName,
                    Description = recipe.Description,
                    Image = recipe.Image,
                    Difficulty = recipe.Difficulty,
                    PersonQuantity = recipe.PersonQuantity,
                    Time = recipe.Time,
                });
            }

            return new DataChunkViewModel<RecipeDto>
            {
                Data = recipeDtos,
                Total = vm.Total
            };
        }
    }
}
