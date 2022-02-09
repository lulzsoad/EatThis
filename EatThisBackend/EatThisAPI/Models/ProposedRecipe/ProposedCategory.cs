using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.ProposedRecipe
{
    public class ProposedCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ProposedCategory(string name)
        {
            Name = name;
        }
    }
}
