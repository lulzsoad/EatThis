using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public bool IsVisible { get; set; }

        public ICollection<IngredientQuantity> IngredientQuantities { get; set; }
        public ICollection<Step> Steps { get; set; }
        public virtual RecipeImage RecipeImage { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
