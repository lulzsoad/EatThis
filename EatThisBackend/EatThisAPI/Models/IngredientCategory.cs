using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class IngredientCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }
    }
}
