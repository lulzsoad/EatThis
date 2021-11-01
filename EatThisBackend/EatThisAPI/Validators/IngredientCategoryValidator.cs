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
    public interface IIngredientCategoryValidator
    {
        void IsNull(IngredientCategory ingredientIngredientCategory);
        Task CheckIfAlreadyExists(IngredientCategory ingredientIngredientCategory);
        Task CheckIfNotFound(int id);
    }

    public class IngredientCategoryValidator : IIngredientCategoryValidator
    {
        private readonly IIngredientCategoryRepository ingredientIngredientCategoryRepository;
        public IngredientCategoryValidator(IIngredientCategoryRepository ingredientIngredientCategoryRepository)
        {
            this.ingredientIngredientCategoryRepository = ingredientIngredientCategoryRepository;
        }

        public void IsNull(IngredientCategory ingredientIngredientCategory)
        {
            if (ingredientIngredientCategory == null)
            {
                throw new CustomException(BackendMessage.General.INVALID_OBJECT_MODEL);
            }
        }

        public async Task CheckIfNotFound(int id)
        {
            if (!await ingredientIngredientCategoryRepository.CheckIfExistsById(id))
            {
                throw new CustomException(BackendMessage.Category.CATEGORY_NOT_FOUND);
            }
        }

        public async Task CheckIfAlreadyExists(IngredientCategory ingredientIngredientCategory)
        {
            if (await ingredientIngredientCategoryRepository.CheckIfExists(ingredientIngredientCategory))
            {
                throw new CustomException(BackendMessage.Category.CATEGORY_ALREADY_EXISTS);
            }
        }
    }
}
