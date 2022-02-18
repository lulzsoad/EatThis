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
using System.Text.Json;
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
        Task<ProposedRecipeDto> GetProposedRecipeById(int id);
        Task<DataChunkViewModel<RecipeDtoByIngredientsViewModel>> GetRecipesByIngredients(string ingredientsJson, int? skip, int? take);
        Task<List<RecipeDto>> GetCurrentUsersRecipe();
        Task AcceptProposedCategory(int proposedRecipeId, ProposedCategoryDto proposedCategory);
        Task ProposedRecipeRemoveProposedCategory(int proposedRecipeId, int proposedCategoryId);
        Task ChangeProposedIngredientToIngredient(int proposedRecipe, int proposedIngredientId, int ingredientId);
        Task<int> AcceptProposedRecipe(ProposedRecipeDto proposedRecipeDto);
        Task DiscardProposedRecipe(DiscardProposedRecipeViewModel discardProposedRecipeViewModel);
    }
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository recipeRepository;
        private readonly IValidator validator;
        private readonly IUserHelper userHelper;
        private readonly IRecipeValidator recipeValidator;
        private readonly ICategoryService categoryService;
        private readonly IEmailService emailService;
        public RecipeService(
            IRecipeRepository recipeRepository,
            IValidator validator,
            IUserHelper userHelper,
            IRecipeValidator recipeValidator,
            ICategoryService categoryService,
            IEmailService emailService
            )
        {
            this.recipeRepository = recipeRepository;
            this.validator = validator;
            this.userHelper = userHelper;
            this.recipeValidator = recipeValidator;
            this.categoryService = categoryService;
            this.emailService = emailService;
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
                Description = proposedRecipeDto.Description,
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

        public async Task<ProposedRecipeDto> GetProposedRecipeById(int id)
        {
            validator.IsObjectNull(id);
            var proposedRecipe = await recipeRepository.GetProposedRecipeById(id);
            recipeValidator.DoesRecipeExists(proposedRecipe);
            var proposedRecipeDto = new ProposedRecipeDto
            {
                Id = proposedRecipe.Id,
                Name = proposedRecipe.Name,
                SubName = proposedRecipe.SubName,
                Description = proposedRecipe.Description,
                Difficulty = proposedRecipe.Difficulty,
                Image = proposedRecipe.Image,
                Note = proposedRecipe.Note,
                PersonQuantity = proposedRecipe.PersonQuantity,
                Time = proposedRecipe.Time,
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
                    LastName = proposedRecipe.User.LastName,
                    Email = proposedRecipe.User.Email,
                    Image = proposedRecipe.User.Image
                },
                ProposedIngredientQuantities = proposedRecipe.ProposedIngredientQuantities.Where(y => y.ProposedIngredient != null).Select(x => new ProposedIngredientQuantityDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    Quantity = x.Quantity,
                    Unit = new UnitDto
                    {
                        Id = x.Unit.Id,
                        Name = x.Unit.Name
                    },
                    ProposedIngredient = new ProposedIngredientDto
                    {
                        Id = x.ProposedIngredient.Id,
                        Name = x.ProposedIngredient.Name,
                        IngredientCategory = new IngredientCategoryDto
                        {
                            Id = x.ProposedIngredient.IngredientCategory.Id,
                            Name = x.ProposedIngredient.IngredientCategory.Name
                        },
                    }
                }).ToList(),
                IngredientQuantities = proposedRecipe.ProposedIngredientQuantities.Where(y => y.Ingredient != null).Select(x => new IngredientQuantityDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    Quantity = x.Quantity,
                    Unit = new UnitDto
                    {
                        Id = x.Unit.Id,
                        Name = x.Unit.Name
                    },
                    Ingredient = new IngredientDto
                    {
                        Id = x.Ingredient.Id,
                        Name = x.Ingredient.Name,
                        IngredientCategory = new IngredientCategoryDto
                        {
                            Id = x.Ingredient.IngredientCategory.Id,
                            Name = x.Ingredient.IngredientCategory.Name
                        },
                    }
                }).Where(x => x != null).ToList(),
                ProposedSteps = proposedRecipe.ProposedSteps.Select(x => new StepDto
                {
                    Id = x.Id,
                    Description = x.Description,
                    Image = x.Image,
                    Order = x.Order
                }).ToList()
            };

            return proposedRecipeDto;
        }

        public async Task<DataChunkViewModel<RecipeDtoByIngredientsViewModel>> GetRecipesByIngredients(string ingredientsJson, int? skip, int? take)
        {
            var ingredientsDto = JsonSerializer.Deserialize<List<IngredientDto>>(ingredientsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
            var ingredients = new List<Ingredient>();
            foreach(var ingredientDto in ingredientsDto)
            {
                ingredients.Add(new Ingredient
                {
                    Id = ingredientDto.Id,
                    Name = ingredientDto.Name
                });
            }

            var recipes = await recipeRepository.GetRecipesByIngredients(ingredients, skip, take);
            var recipesDto = new DataChunkViewModel<RecipeDtoByIngredientsViewModel>() 
            { 
                Data = new List<RecipeDtoByIngredientsViewModel>(),
                Total = 0
            };

            foreach(var data in recipes.Data)
            {
                recipesDto.Data.Add(new RecipeDtoByIngredientsViewModel
                {
                    Recipe = new RecipeDto
                    {
                        Id = data.Recipe.Id,
                        Name = data.Recipe.Name,
                        SubName = data.Recipe.SubName,
                        Description = data.Recipe.Description,
                        Difficulty = data.Recipe.Difficulty,
                        Time = data.Recipe.Time,
                        PersonQuantity = data.Recipe.PersonQuantity,
                        Image = data.Recipe.Image,
                        Category = new CategoryDto
                        {
                            Id = data.Recipe.Category.Id,
                            Name = data.Recipe.Category.Name
                        },
                        IngredientQuantities = new List<IngredientQuantityDto>()
                    },

                    IncludedIngredientsCount = data.IncludedIngredientsCount,
                    MissingIngredientsCount = data.MissingIngredientsCount
                });

                foreach (var ingredientQuantity in data.Recipe.IngredientQuantities)
                {
                    recipesDto.Data.FirstOrDefault(x => x.Recipe.Id == data.Recipe.Id).Recipe.IngredientQuantities.Add(new IngredientQuantityDto 
                    { 
                        Ingredient = new IngredientDto
                        {
                            Id = ingredientQuantity.Ingredient.Id,
                            Name = ingredientQuantity.Ingredient.Name
                        }
                    });
                }
            }

            

            recipesDto.Total = recipes.Total;
            return recipesDto;
        }

        public async Task<List<RecipeDto>> GetCurrentUsersRecipe()
        {
            var userId = userHelper.GetCurrentUserId();
            var recipes = await recipeRepository.GetRecipesByUserId(userId);
            var recipesDto = new List<RecipeDto>();
            foreach(var recipe in recipes)
            {
                recipesDto.Add(new RecipeDto
                {
                    Id = recipe.Id,
                    Name = recipe.Name
                });
            };

            return recipesDto;
        }

        public async Task AcceptProposedCategory(int proposedRecipeId, ProposedCategoryDto proposedCategory)
        {
            validator.IsObjectNull(proposedCategory);
            var category = new CategoryDto
            {
                Name = proposedCategory.Name
            };
            int id = await categoryService.Add(category);
            await recipeRepository.ProposedRecipeChangeCategory(proposedRecipeId, id);
            await recipeRepository.ProposedCategoryRemoveProposedCategory(proposedRecipeId, proposedCategory.Id);
        }

        public async Task ProposedRecipeRemoveProposedCategory(int proposedRecipeId, int proposedCategoryId)
        {
            await recipeRepository.ProposedCategoryRemoveProposedCategory(proposedRecipeId, proposedCategoryId);
        }

        public async Task ChangeProposedIngredientToIngredient(int proposedRecipe, int proposedIngredientId, int ingredientId)
        {
            await recipeRepository.ChangeProposedIngredientToIngredient(proposedRecipe, proposedIngredientId, ingredientId);
        }

        public async Task<int> AcceptProposedRecipe(ProposedRecipeDto proposedRecipeDto)
        {
            validator.IsObjectNull(proposedRecipeDto);
            var recipe = new Recipe
            {
                Name = proposedRecipeDto.Name,
                SubName = proposedRecipeDto.SubName,
                CategoryId = proposedRecipeDto.Category.Id,
                Time = proposedRecipeDto.Time,
                PersonQuantity = proposedRecipeDto.PersonQuantity,
                Difficulty = proposedRecipeDto.Difficulty,
                Description = proposedRecipeDto.Description,
                CreationDate = DateTime.UtcNow,
                IsVisible = true,
                Image = proposedRecipeDto.Image,
                UserId = proposedRecipeDto.UserDetails.Id,
            };

            var ingredientQuantities = new List<IngredientQuantity>();
            foreach(var ingredientQuantity in proposedRecipeDto.IngredientQuantities)
            {
                ingredientQuantities.Add(new IngredientQuantity
                {
                    IngredientId = ingredientQuantity.Ingredient.Id,
                    Description = ingredientQuantity.Description,
                    Quantity = ingredientQuantity.Quantity,
                    UnitId = ingredientQuantity.Unit.Id
                });
            }

            var steps = new List<Step>();
            foreach(var step in proposedRecipeDto.ProposedSteps)
            {
                steps.Add(new Step
                {
                    Order = step.Order,
                    Description = step.Description,
                    Image = step.Image,
                });
            }

            int recipeId = await recipeRepository.AcceptProposedRecipe(recipe, ingredientQuantities, steps, proposedRecipeDto.Id);

            emailService.SendByNoReply(
                proposedRecipeDto.UserDetails.Email, 
                "Twój przepis został zaakceptowany", 
                BackendMessage.EmailMessages.EMAILMESSAGE_YOUR_RECIPE_HAS_BEEN_ACCEPTED
                );

            return recipeId;
        }

        public async Task DiscardProposedRecipe(DiscardProposedRecipeViewModel discardProposedRecipeViewModel)
        {
            validator.IsObjectNull(discardProposedRecipeViewModel);
            await recipeRepository.DiscardProposedRecipe(discardProposedRecipeViewModel.ProposedRecipeId);

            emailService.SendByNoReply(
                discardProposedRecipeViewModel.Email,
                "Twój przepis został odrzucony",
                $"Powód odrzucenia: {discardProposedRecipeViewModel.Message}"
                );
        }
    }
}
