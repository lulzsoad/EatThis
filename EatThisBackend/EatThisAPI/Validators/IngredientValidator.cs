using EatThisAPI.Exceptions;
using EatThisAPI.Helpers;
using EatThisAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Validators
{
    public static class IngredientValidator
    {
        public static void Validate(Ingredient ingredient)
        {
            if(ingredient == null)
            {
                throw new CustomException(BackendMessage.Ingredient.INGREDIENT_NOT_FOUND);
            }
        }
    }
}
