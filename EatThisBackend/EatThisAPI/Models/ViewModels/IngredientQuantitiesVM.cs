using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.ProposedRecipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.ViewModels
{
    public class IngredientQuantitiesVM
    {
        public List<IngredientQuantityDto> IngredientQuantities { get; set; }
        public List<ProposedIngredientQuantityDto> ProposedIngredientQuantities { get; set; }
    }
}
