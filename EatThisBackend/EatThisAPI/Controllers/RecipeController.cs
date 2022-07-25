using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.ProposedRecipe;
using EatThisAPI.Models.ViewModels;
using EatThisAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        public async Task<ActionResult<int>> Add([FromBody] RecipeDto recipeDto)
        {
            return Ok(await recipeService.AddRecipe(recipeDto));
        }

        [HttpGet]
        public async Task<ActionResult<DataChunkViewModel<RecipeDto>>> GetRecipesByCategory([FromQuery] string categoryId, [FromQuery] int skip, [FromQuery] int take)
        {
            return Ok(await recipeService.GetChunkOfRecipesByCategory(categoryId, skip, take));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<RecipeDto>> GetRecipe([FromRoute] int id)
        {
            return Ok(await recipeService.GetRecipeById(id));
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<ActionResult<RecipeDto>> PatchRecipe([FromBody] JsonPatchDocument recipeDto, [FromRoute] int id)
        {
            return Ok();
        }

        [HttpGet]
        [Route("byIngredients")]
        public async Task<ActionResult> GetRecipesByIngredients([FromQuery] string ingredients, [FromQuery] int? skip, [FromQuery] int? take)
        {
            return Ok(await recipeService.GetRecipesByIngredients(ingredients, skip, take));
        }

        [Authorize]
        [HttpPost]
        [Route("addProposedRecipe")]
        public async Task<ActionResult<int>> AddProposedRecipe([FromBody] ProposedRecipeDto proposedRecipeDto)
        {
            return Ok(await recipeService.AddProposedRecipe(proposedRecipeDto));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        [Route("proposedRecipes")]
        public async Task<ActionResult<List<ProposedRecipeDto>>> GetProposedRecipes([FromQuery]int skip, [FromQuery]int take)
        {
            return Ok(await recipeService.GetChunkOfProposedRecipes(skip, take));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        [Route("proposedRecipes/{id}")]
        public async Task<ActionResult<List<ProposedRecipeDto>>> GetProposedRecipeById([FromRoute] int id)
        {
            return Ok(await recipeService.GetProposedRecipeById(id));
        }

        

        [Authorize]
        [HttpGet]
        [Route("currentUserRecipes")]
        public async Task<ActionResult> GetCurentUserRecipess()
        {
            return Ok(await recipeService.GetCurrentUsersRecipe());
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPut]
        [Route("acceptProposedCategory/{id}")]
        public async Task<ActionResult> AcceptProposedCategory([FromRoute]int id, [FromBody] ProposedCategoryDto proposedCategory)
        {
            await recipeService.AcceptProposedCategory(id, proposedCategory);
            return Ok();
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete]
        [Route("removeProposedCategory")]
        public async Task<ActionResult> ProposedRecipeRemoveProposedCategory([FromQuery] int proposedRecipeId, [FromQuery] int proposedCategoryId)
        {
            await recipeService.ProposedRecipeRemoveProposedCategory(proposedRecipeId, proposedCategoryId);
            return Ok();
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPut]
        [Route("changeProposedIngredientToIngredient")]
        public async Task<ActionResult> ChangeProposedIngredientToIngrdient([FromQuery] int proposedRecipeId, [FromQuery] int proposedIngredientId, [FromQuery] int ingredientId)
        {
            await recipeService.ChangeProposedIngredientToIngredient(proposedRecipeId, proposedIngredientId, ingredientId);
            return Ok();
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        [Route("acceptProposedRecipe")]
        public async Task<ActionResult> AcceptProposedRecipe([FromBody] ProposedRecipeDto proposedRecipeDto)
        {
            return Ok(await recipeService.AcceptProposedRecipe(proposedRecipeDto));
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpDelete]
        [Route("discardProposedRecipe")]
        public async Task<ActionResult> DiscardProposedRecipe([FromBody] DiscardProposedRecipeViewModel discardProposedRecipeRequest)
        {
            await recipeService.DiscardProposedRecipe(discardProposedRecipeRequest);
            return Ok();
        }
    }
}
