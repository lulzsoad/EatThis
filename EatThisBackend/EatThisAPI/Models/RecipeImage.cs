using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class RecipeImage
    {
        [ForeignKey("Recipe")]
        public int Id { get; set; }
        public string Url { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
