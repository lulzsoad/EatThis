using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs
{
    public class IngredientQuantityDto
    {
        public int Id { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public IngredientDto Ingredient { get; set; }
        public UnitDto Unit { get; set; }
    }
}
