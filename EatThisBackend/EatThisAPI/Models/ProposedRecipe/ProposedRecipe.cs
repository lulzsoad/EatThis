using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.ProposedRecipe
{
    public class ProposedRecipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubName { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public int Time { get; set; }
        public int PersonQuantity { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public string Note { get; set; }

        public ICollection<ProposedIngredientQuantity> ProposedIngredientQuantities { get; set; }
        public ICollection<ProposedStep> ProposedSteps { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? ProposedCategoryId { get; set; }
        public ProposedCategory ProposedCategory { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
