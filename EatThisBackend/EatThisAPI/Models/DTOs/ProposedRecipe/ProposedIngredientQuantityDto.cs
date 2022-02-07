using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs.ProposedRecipe
{
    public class ProposedIngredientQuantityDto
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public ProposedIngredientDto ProposedIngredient { get; set; }
        public UnitDto Unit { get; set; }
    }
}
