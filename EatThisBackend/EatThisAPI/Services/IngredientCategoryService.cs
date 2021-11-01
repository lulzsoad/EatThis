using AutoMapper;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IIngredientCategoryService
    {
        Task<IEnumerable<IngredientCategoryDto>> GetAll();
        Task<IngredientCategoryDto> GetById(int id);
        Task<int> Add(IngredientCategoryDto ingredientCategoryDto);
        Task<IngredientCategoryDto> Update(IngredientCategoryDto IngredientCategoryDto);
        Task Delete(IngredientCategoryDto ingredientCategoryDto);
    }
    public class IngredientCategoryService : IIngredientCategoryService
    {
        private readonly IIngredientCategoryRepository ingredientCategoryRepository;
        private readonly IMapper mapper;
        private readonly ILogger<IngredientCategoryService> logger;
        private readonly IIngredientCategoryValidator ingredientCategoryValidator;

        public IngredientCategoryService(
            IIngredientCategoryRepository ingredientCategoryRepository, 
            IMapper mapper, ILogger<IngredientCategoryService> logger,
            IIngredientCategoryValidator ingredientCategoryValidator
            )
        {
            this.ingredientCategoryRepository = ingredientCategoryRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.ingredientCategoryValidator = ingredientCategoryValidator;
        }

        public async Task<IEnumerable<IngredientCategoryDto>> GetAll()
        {
            IEnumerable<IngredientCategory> ingredientCategories = await ingredientCategoryRepository.GetAll();
            IEnumerable<IngredientCategoryDto> ingredientCategoryDtos = mapper.Map<IEnumerable<IngredientCategoryDto>>(ingredientCategories);
            return ingredientCategoryDtos;
        }

        public async Task<IngredientCategoryDto> GetById(int id)
        {
            await ingredientCategoryValidator.CheckIfNotFound(id);

            IngredientCategory ingredientCategory = await ingredientCategoryRepository.GetById(id);
            var ingredientCategoryDto = mapper.Map<IngredientCategoryDto>(ingredientCategory);
            return ingredientCategoryDto;
        }

        public async Task<int> Add(IngredientCategoryDto ingredientCategoryDto)
        {
            var ingredientCategory = mapper.Map<IngredientCategory>(ingredientCategoryDto);
            ingredientCategoryValidator.IsNull(ingredientCategory);
            await ingredientCategoryValidator.CheckIfAlreadyExists(ingredientCategory);

            int id = await ingredientCategoryRepository.Add(ingredientCategory);
            return id;
        }

        public async Task Delete(IngredientCategoryDto ingredientCategoryDto)
        {
            var ingredientCategory = mapper.Map<IngredientCategory>(ingredientCategoryDto);
            ingredientCategoryValidator.IsNull(ingredientCategory);
            await ingredientCategoryRepository.Delete(ingredientCategory);
        }

        public async Task<IngredientCategoryDto> Update(IngredientCategoryDto ingredientCategoryDto)
        {
            var ingredientCategory = mapper.Map<IngredientCategory>(ingredientCategoryDto);
            ingredientCategoryValidator.IsNull(ingredientCategory);
            await ingredientCategoryValidator.CheckIfNotFound(ingredientCategoryDto.Id);
            ingredientCategory = await ingredientCategoryRepository.Update(ingredientCategory);
            ingredientCategoryDto.Name = ingredientCategory.Name;
            return ingredientCategoryDto;
        }
    }
}
