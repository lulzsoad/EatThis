﻿using EatThisAPI.Models.ProposedRecipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int IngredientCategoryId { get; set; }
        public IngredientCategory IngredientCategory { get; set; }
        //public ICollection<IngredientQuantity> IngredientQuantities { get; set; }
        //public ICollection<ProposedIngredientQuantity> ProposedIngredientQuantities { get; set; }
    }
}
