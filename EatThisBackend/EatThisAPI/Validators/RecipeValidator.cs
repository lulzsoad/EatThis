using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Validators
{
    public interface IRecipeValidator
    {
        void DoesRecipeExists(object recipe);
        void ValidateRecipe(Recipe recipe);
    }
    public class RecipeValidator : IRecipeValidator
    {
        public void DoesRecipeExists(object recipe)
        {
            if(recipe == null)
            {
                throw new CustomException(BackendMessage.Recipe.RECIPE_NOT_FOUND);
            }
        }

        public void ValidateRecipe(Recipe recipe)
        {

        }
    }
}
