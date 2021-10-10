using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class Step
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public virtual Image Image { get; set; }
    }
}
