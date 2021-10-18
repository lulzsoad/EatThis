using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Validators
{
    public interface IIngredientValidator
    {
        void IsNull(Ingredient ingredient);
        Task CheckIfAlreadyExists(Ingredient ingredient);
        Task CheckIfNotFound(int id);
    }

    public class IngredientValidator : IIngredientValidator
    {
        private readonly IIngredientRepository ingredientRepository;
        public IngredientValidator(IIngredientRepository ingredientRepository)
        {
            this.ingredientRepository = ingredientRepository;
        }

        public void IsNull(Ingredient ingredient)
        {
            if(ingredient == null)
            {
                throw new CustomException(BackendMessage.General.INVALID_OBJECT_MODEL);
            }
        }

        public async Task CheckIfNotFound(int id)
        {
            if (!await ingredientRepository.CheckIfExistsById(id))
            {
                throw new CustomException(BackendMessage.Ingredient.INGREDIENT_NOT_FOUND);
            }
        }

        public async Task CheckIfAlreadyExists(Ingredient ingredient)
        {
            if (await ingredientRepository.CheckIfExists(ingredient))
            {
                throw new CustomException(BackendMessage.Ingredient.INGREDIENT_ALREADY_EXISTS);
            }
        }
    }
}
