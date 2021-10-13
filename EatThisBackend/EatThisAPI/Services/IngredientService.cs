using AutoMapper;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Repositories;
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
        public IngredientService(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            this.ingredientRepository = ingredientRepository;
            this.mapper = mapper;
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

            if(ingredient == null)
            {
                throw new Exception("Nie znaleziono składnika");
            }
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
