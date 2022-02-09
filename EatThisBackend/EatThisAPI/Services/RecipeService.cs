using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.ProposedRecipe;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Models.ProposedRecipe;
using EatThisAPI.Models.ViewModels;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IRecipeService
    {
        Task<int> AddRecipe(RecipeDto recipeDto);
        Task<int> AddProposedRecipe(ProposedRecipeDto proposedRecipeDto);
        Task<DataChunkViewModel<RecipeDto>> GetChunkOfRecipesByCategory(string categoryId, int skip, int take);
        Task<RecipeDto> GetRecipeById(int recipeId);
        Task<DataChunkViewModel<ProposedRecipeDto>> GetChunkOfProposedRecipes(int skip, int take);
    }
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IValidator validator;
        private readonly IUserHelper userHelper;
        private readonly IRecipeValidator recipeValidator;
        public RecipeService(
            IRecipeRepository recipeRepository,
            IValidator validator,
            IUserHelper userHelper,
            IRecipeValidator recipeValidator
            )
        {
            this.recipeRepository = recipeRepository;
            this.validator = validator;
            this.userHelper = userHelper;
            this.recipeValidator = recipeValidator;
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
                    Ingredient = new Ingredient 
                    {
                        Id = item.Ingredient.Id, 
                        Name = item.Ingredient.Name, 
                        IngredientCategoryId = item.Ingredient.IngredientCategory.Id
                    },

                    Description = item.Description,
                    Unit = new Unit
                    {
                        Id = item.Unit.Id,
                        Name = item.Unit.Name
                    },
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

        public async Task<int> AddProposedRecipe(ProposedRecipeDto proposedRecipeDto)
        {
            validator.IsObjectNull(proposedRecipeDto);
            var user = await this.userHelper.GetCurrentUser();
            
            var proposedRecipe = new ProposedRecipe() 
            { 
                Name = proposedRecipeDto.Name,
                SubName = proposedRecipeDto.SubName,
                Description = proposedRecipeDto.SubName,
                Image = proposedRecipeDto.Image,
                Time = proposedRecipeDto.Time,
                PersonQuantity = proposedRecipeDto.PersonQuantity,
                Difficulty = proposedRecipeDto.Difficulty,
                CategoryId = proposedRecipeDto.Category.Id,
                CreationDate = DateTime.UtcNow,
                Note = proposedRecipeDto.Note,
                UserId = user.Id,
            };

            ProposedCategory proposedCategory = null;
            if (proposedRecipeDto.ProposedCategory != null)
            {
                proposedCategory = new ProposedCategory(proposedRecipeDto.ProposedCategory.Name);
            }

            var proposedIngredients = new List<ProposedIngredient>();
            var proposedIngredientQuantities = new List<ProposedIngredientQuantity>();
            var proposedSteps = new List<ProposedStep>();

            if(proposedRecipeDto.IngredientQuantities.Count > 0)
            {
                foreach(var ingredientQuantity in proposedRecipeDto.IngredientQuantities)
                {
                    proposedIngredientQuantities.Add(new ProposedIngredientQuantity
                    {
                        IngredientId = ingredientQuantity.Ingredient.Id,
                        Description = ingredientQuantity.Description,
                        UnitId = ingredientQuantity.Unit.Id,
                        Quantity = ingredientQuantity.Quantity
                    });
                }
                
            }

            if(proposedRecipeDto.ProposedIngredientQuantities.Count > 0)
            {
                foreach (var proposedIngredientQuantity in proposedRecipeDto.ProposedIngredientQuantities)
                {
                    var reference = Guid.NewGuid().ToString();
                    proposedIngredients.Add(new ProposedIngredient 
                    { 
                        Name = proposedIngredientQuantity.ProposedIngredient.Name,
                        IngredientCategoryId = proposedIngredientQuantity.ProposedIngredient.IngredientCategory.Id,
                        Reference = reference
                    });
                    proposedIngredientQuantities.Add(new ProposedIngredientQuantity 
                    {
                        Description = proposedIngredientQuantity.Description,
                        Quantity = proposedIngredientQuantity.Quantity,
                        UnitId = proposedIngredientQuantity.Unit.Id,
                        Reference = reference
                    });
                }
            }

            foreach(var proposedStep in proposedRecipeDto.ProposedSteps)
            {
                proposedSteps.Add(new ProposedStep
                {
                    Description = proposedStep.Description,
                    Image = proposedStep.Image,
                    Order = proposedStep.Order,
                });
            }

            return await recipeRepository.AddProposedRecipe(proposedRecipe, proposedCategory, proposedIngredients,
                proposedIngredientQuantities, proposedSteps); ;
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

        public async Task<RecipeDto> GetRecipeById(int recipeId)
        {
            validator.IsObjectNull(recipeId);
            var recipe = await recipeRepository.GetRecipeById(recipeId);
            recipeValidator.DoesRecipeExists(recipe);

            var recipeDto = new RecipeDto
            {
                Id = recipe.Id,
                Category = new CategoryDto 
                { 
                    Id = recipe.Category.Id,
                    Name = recipe.Category.Name
                },
                Difficulty = recipe.Difficulty,
                Name = recipe.Name,
                Description = recipe.Description,
                Image = recipe.Image,
                IngredientQuantities = new List<IngredientQuantityDto>(),
                IsVisible = recipe.IsVisible,
                PersonQuantity = recipe.PersonQuantity,
                Steps = new List<StepDto>(),
                SubName = recipe.SubName,
                Time = recipe.Time,
                UserDetails = new UserDetails
                {
                    Id = recipe.User.Id,
                    FirstName = recipe.User.FirstName,
                    LastName = recipe.User.LastName,
                    Image = recipe.User.Image
                }
                
            };

            foreach(var ingredientQuantity in recipe.IngredientQuantities)
            {
                recipeDto.IngredientQuantities.Add(new IngredientQuantityDto
                {
                    Id = ingredientQuantity.Id,
                    Description = ingredientQuantity.Description,
                    Quantity = ingredientQuantity.Quantity,
                    Ingredient = new IngredientDto
                    {
                        Id = ingredientQuantity.Ingredient.Id,
                        Name = ingredientQuantity.Ingredient.Name,
                        IngredientCategory = new IngredientCategoryDto
                        {
                            Id = ingredientQuantity.Ingredient.IngredientCategory.Id,
                            Name = ingredientQuantity.Ingredient.IngredientCategory.Name
                        },
                    },
                    Unit = new UnitDto
                    {
                        Id = ingredientQuantity.Unit.Id,
                        Name = ingredientQuantity.Unit.Name
                    }
                });
            }

            foreach(var step in recipe.Steps)
            {
                recipeDto.Steps.Add(new StepDto
                {
                    Id = step.Id,
                    Description = step.Description,
                    Image = step.Image,
                    Order = step.Order
                });
            }

            recipeDto.Steps = recipeDto.Steps.OrderBy(x => x.Order).ToList();

            return recipeDto;
        }

        public async Task<RecipeDto> PatchRecipe(JsonPatchDocument recipeDto, int id)
        {
            return new RecipeDto();
;       }

        public async Task<DataChunkViewModel<ProposedRecipeDto>> GetChunkOfProposedRecipes(int skip, int take)
        {
            var proposedRecipes = await recipeRepository.GetChunkOfProposedRecipes(skip, take);
            List<ProposedRecipeDto> proposedRecipesDto = new List<ProposedRecipeDto>();
            foreach(var proposedRecipe in proposedRecipes.Data)
            {
                proposedRecipesDto.Add(new ProposedRecipeDto
                {
                    Id = proposedRecipe.Id,
                    Name = proposedRecipe.Name,
                    ProposedCategory = proposedRecipe.ProposedCategory != null ? new ProposedCategoryDto
                    {
                        Id = proposedRecipe.ProposedCategory.Id,
                        Name = proposedRecipe.ProposedCategory.Name
                    } : null,
                    Category = new CategoryDto 
                    { 
                        Id = proposedRecipe.Category.Id,
                        Name = proposedRecipe.Category.Name
                    },
                    UserDetails = new UserDetails
                    {
                        Id = proposedRecipe.User.Id,
                        FirstName = proposedRecipe.User.FirstName,
                        LastName = proposedRecipe.User.LastName
                    }
                });
            }

            return new DataChunkViewModel<ProposedRecipeDto> { Data = proposedRecipesDto, Total = proposedRecipes.Total };
        }
    }
}
