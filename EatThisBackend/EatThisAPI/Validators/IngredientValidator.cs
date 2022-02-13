using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using EatThisAPI.Models.ProposedRecipe;
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
        Task CheckIfNotFound(Ingredient ingredient);
        Task CheckIfNotFound(ProposedIngredient proposedIngredient);
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

        public async Task CheckIfNotFound(Ingredient ingredient)
        {
            if (!await ingredientRepository.CheckIfExistsById(ingredient.Id))
            {
                throw new CustomException(BackendMessage.Ingredient.INGREDIENT_NOT_FOUND);
            }
        }

        public async Task CheckIfNotFound(ProposedIngredient proposedIngredient)
        {
            if (!await ingredientRepository.CheckIfExists(proposedIngredient))
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
