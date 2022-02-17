using EatThisAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.ViewModels
{
    public class RecipeByIngredientsViewModel
    {
        public Recipe Recipe { get; set; }
        public int IncludedIngredientsCount { get; set; }
        public int MissingIngredientsCount { get; set; }
    }

    public class RecipeDtoByIngredientsViewModel
    {
        public RecipeDto Recipe { get; set; }
        public int IncludedIngredientsCount { get; set; }
        public int MissingIngredientsCount { get; set; }
    }
}
