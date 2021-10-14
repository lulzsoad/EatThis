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

        public IngredientService(IIngredientRepository ingredientRepository, IMapper mapper, ILogger<IngredientService> logger)
        {
            this.ingredientRepository = ingredientRepository;
            this.mapper = mapper;
            this.logger = logger;
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

            IngredientValidator.Validate(ingredient);
            var ingredientDto = mapper.Map<IngredientDto>(ingredient);
            return ingredientDto;
        }

        public async Task<int> Add(IngredientDto ingredient)
        {
            if(ingredient == null)
            {
                throw new Exception("Nieprawidłowy model obiektu");
            }

            int id = await ingredientRepository.Add(new Ingredient { Id = ingredient.Id, Name = ingredient.Name });
            return id;
        }

        public async Task Delete(IngredientDto ingredientDto)
        {
            if(ingredientDto == null)
            {
               throw new Exception("Nieprawidłowy model obiektu");
            }

            await ingredientRepository.Delete(new Ingredient { Id = ingredientDto.Id, Name = ingredientDto.Name });
        }

        public async Task<IngredientDto> Update(IngredientDto ingredientDto)
        {
            if (ingredientDto == null)
            {
                throw new Exception("Nieprawidłowy model obiektu");
            }

            Ingredient ingredient = new Ingredient { Id = ingredientDto.Id, Name = ingredientDto.Name };
            ingredient = await ingredientRepository.Update(ingredient);
            ingredientDto.Name = ingredient.Name;
            return ingredientDto;
        }
    }
}
