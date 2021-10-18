using AutoMapper;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
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
    }
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository ingredientRepository;
        private readonly IMapper mapper;
        private readonly ILogger<IngredientService> logger;
        private readonly IIngredientValidator ingredientValidator;

        public IngredientService(IIngredientRepository ingredientRepository, 
            IMapper mapper, 
            ILogger<IngredientService> logger, 
            IIngredientValidator ingredientValidator)
        {
            this.ingredientRepository = ingredientRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.ingredientValidator = ingredientValidator;
        }

        public async Task<IEnumerable<IngredientDto>> GetAll() 
        {
            IEnumerable<Ingredient> ingredients = await ingredientRepository.GetAll();
            IEnumerable<IngredientDto> ingredientDtos = mapper.Map<IEnumerable<IngredientDto>>(ingredients);
            return ingredientDtos;
        }

        public async Task<IngredientDto> GetById(int id)
        {
            await ingredientValidator.CheckIfNotFound(id);

            Ingredient ingredient = await ingredientRepository.GetById(id);
            var ingredientDto = mapper.Map<IngredientDto>(ingredient);
            return ingredientDto;
        }

        public async Task<int> Add(IngredientDto ingredientDto)
        {
            var ingredient = mapper.Map<Ingredient>(ingredientDto);

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
            var ingredient = mapper.Map<Ingredient>(ingredientDto);
            ingredientValidator.IsNull(ingredient);
            await ingredientValidator.CheckIfNotFound(ingredientDto.Id);
            ingredient = await ingredientRepository.Update(ingredient);
            ingredientDto.Name = ingredient.Name;
            return ingredientDto;
        }
    }
}
