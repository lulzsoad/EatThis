using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public CategoryDto Category { get; set; }
        public IngredientQuantity[] IngredientQuantities { get; set; }

    }
}
