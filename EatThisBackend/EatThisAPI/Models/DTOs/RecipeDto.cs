using EatThisAPI.Models.DTOs.User;
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
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public bool IsVisible { get; set; }
        public List<IngredientQuantity> IngredientQuantities { get; set; }

        public List<Step> Steps { get; set; }
        public CategoryDto Category { get; set; }
        public string Image { get; set; }
        public int Time { get; set; }
        public int PersonQuantity { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}
