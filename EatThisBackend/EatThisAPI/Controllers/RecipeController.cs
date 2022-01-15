using EatThisAPI.Models.DTOs;
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
        public async Task<ActionResult<RecipeDto>> PatchRecipe([FromBody]JsonPatchDocument recipeDto, [FromRoute]int id)
        {
            return Ok();
        }
    }
}
