using AutoMapper;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.ProposedRecipe;
using EatThisAPI.Models.ViewModels;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDto>> GetAll();
        Task<IngredientDto> GetById(int id);
        Task<int> Add(IngredientDto ingredientDto);
        Task<IngredientDto> Update(IngredientDto IngredientDto);
        Task Delete(IngredientDto ingredientDto);
        Task<IngredientQuantityDto> AcceptProposedIngredient(int id);
        Task<IngredientQuantitiesVM> GetProposedIngredientQuantitiesByProposedRecipeId(int proposedRecipeId);
        Task DeleteProposedIngredient(int id);
    }
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository ingredientRepository;
        private readonly IMapper mapper;
        private readonly ILogger<IngredientService> logger;
        private readonly IIngredientValidator ingredientValidator;
        private readonly IValidator validator;

        public IngredientService(IIngredientRepository ingredientRepository, 
            IMapper mapper, 
            ILogger<IngredientService> logger, 
            IIngredientValidator ingredientValidator,
            IValidator validator)
        {
            this.ingredientRepository = ingredientRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.ingredientValidator = ingredientValidator;
            this.validator = validator;
        }

        public async Task<IEnumerable<IngredientDto>> GetAll() 
        {
            IEnumerable<Ingredient> ingredients = await ingredientRepository.GetAll();
            IEnumerable<IngredientDto> ingredientDtos = mapper.Map<IEnumerable<IngredientDto>>(ingredients);
            return ingredientDtos;
        }

        public async Task<IngredientDto> GetById(int id)
        {
            Ingredient ingredient = await ingredientRepository.GetById(id);
            await ingredientValidator.CheckIfNotFound(ingredient);
            var ingredientDto = mapper.Map<IngredientDto>(ingredient);
            return ingredientDto;
        }

        public async Task<int> Add(IngredientDto ingredientDto)
        {
            var ingredient = new Ingredient
            {
                Name = ingredientDto.Name,
                IngredientCategoryId = ingredientDto.IngredientCategory.Id
            };

            ingredientValidator.IsNull(ingredient);
            await ingredientValidator.CheckIfAlreadyExists(ingredient);

            int id = await ingredientRepository.Add(ingredient);
            return id;
        }

        public async Task Delete(IngredientDto ingredientDto)
        {
            var ingredient = mapper.Map<Ingredient>(ingredientDto);
            ingredientValidator.IsNull(ingredient);
            await ingredientRepository.Delete(ingredient);
        }

        public async Task<IngredientDto> Update(IngredientDto ingredientDto)
        {
            var ingredient = new Ingredient
            {
                Id = ingredientDto.Id,
                Name = ingredientDto.Name,
                IngredientCategoryId = ingredientDto.IngredientCategory.Id
            };
            ingredientValidator.IsNull(ingredient);
            await ingredientValidator.CheckIfNotFound(ingredient);
            ingredient = await ingredientRepository.Update(ingredient);
            ingredientDto.Name = ingredient.Name;
            return ingredientDto;
        }

        public async Task<IngredientQuantityDto> AcceptProposedIngredient(int id)
        {
            validator.IsObjectNull(id);
            var ingredientQuantityDto = new IngredientQuantityDto();
            var proposedIngredient = await ingredientRepository.GetProposedIngredientById(id);
            await ingredientValidator.CheckIfNotFound(proposedIngredient);
            var ingredient = new Ingredient
            {
                Name = proposedIngredient.Name,
                IngredientCategoryId = proposedIngredient.IngredientCategoryId
            };
            await ingredientValidator.CheckIfAlreadyExists(ingredient);
            ingredient = await ingredientRepository.AcceptProposedIngredient(proposedIngredient);

            var proposedIngredientQuantity = await ingredientRepository.GetProposedIngredientQuantityByReference(proposedIngredient.Reference);
            ingredientQuantityDto.Id = proposedIngredientQuantity.Id;
            ingredientQuantityDto.Quantity = proposedIngredientQuantity.Quantity;
            ingredientQuantityDto.Description = proposedIngredientQuantity.Description;
            ingredientQuantityDto.Unit = new UnitDto
            {
                Id = proposedIngredientQuantity.Unit.Id,
                Name = proposedIngredientQuantity.Unit.Name
            };
            ingredientQuantityDto.Ingredient = new IngredientDto
            {
                Id = proposedIngredientQuantity.Ingredient.Id,
                Name = proposedIngredientQuantity.Ingredient.Name,
                IngredientCategory = new IngredientCategoryDto
                {
                    Id = proposedIngredientQuantity.Ingredient.IngredientCategory.Id,
                    Name = proposedIngredientQuantity.Ingredient.IngredientCategory.Name,
                }
            };

            return ingredientQuantityDto;
        }

        public async Task<IngredientQuantitiesVM> GetProposedIngredientQuantitiesByProposedRecipeId(int proposedRecipeId)
        {
            var proposedIngredientQuantities = await ingredientRepository.GetProposedIngredientQuantitiesByProposedRecipeId(proposedRecipeId);
            var vm = new IngredientQuantitiesVM()
            {
                IngredientQuantities = proposedIngredientQuantities.Where(y => y.Ingredient != null).Select(x => new IngredientQuantityDto
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
                ProposedIngredientQuantities = proposedIngredientQuantities.Where(y => y.ProposedIngredient != null).Select(x => new ProposedIngredientQuantityDto
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
            };

            return vm;
        }

        public async Task DeleteProposedIngredient(int id)
        {
            await ingredientRepository.DeleteProposedIngredient(id);
        }
    }
}
