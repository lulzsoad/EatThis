﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.ProposedRecipe
{
    public class ProposedIngredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }

        public int IngredientCategoryId { get; set; }
        public IngredientCategory IngredientCategory { get; set; }
        //public ICollection<IngredientQuantity> IngredientQuantities { get; set; }
    }
}
