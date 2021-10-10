using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class IngredientQuantity
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

    }
}
