using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.ProposedRecipe
{
    public class ProposedIngredientQuantity
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public int ProposedIngredientId { get; set; }
        public ProposedIngredient ProposedIngredient { get; set; }
        public int ProposedRecipeId { get; set; }
        public ProposedRecipe Recipe { get; set; }
    }
}
