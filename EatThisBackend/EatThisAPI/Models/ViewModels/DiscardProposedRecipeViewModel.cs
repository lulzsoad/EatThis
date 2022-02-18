using EatThisAPI.Models.DTOs.ProposedRecipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.ViewModels
{
    public class DiscardProposedRecipeViewModel
    {
        public int ProposedRecipeId { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
    }
}
