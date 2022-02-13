using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.ProposedRecipe;
using EatThisAPI.Models.ViewModels;
using EatThisAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService ingredientService;
        public IngredientController(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetAll()
        {
            return Ok(await ingredientService.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IngredientDto>> GetById([FromRoute] int id)
        {
            return Ok(await ingredientService.GetById(id));
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<int>> Add([FromBody] IngredientDto ingredientDto)
        {
            return Ok(await ingredientService.Add(ingredientDto));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> Delete([FromBody] IngredientDto ingredientDto)
        {
            await ingredientService.Delete(ingredientDto);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<IngredientDto>> Update([FromBody] IngredientDto ingredientDto)
        {
            return Ok(await ingredientService.Update(ingredientDto));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        [Route("accept/{id}")]
        public async Task<ActionResult<IngredientQuantityDto>> AcceptProposedIngredient([FromRoute]int id)
        {
            return Ok(await ingredientService.AcceptProposedIngredient(id));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        [Route("proposedRecipeIngredientQuantities/{id}")]
        public async Task<ActionResult<IngredientQuantitiesVM>> GetIngredientQuantitiesByProposedRecipeId([FromRoute]int id)
        {
            return Ok(await ingredientService.GetProposedIngredientQuantitiesByProposedRecipeId(id));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Employee")]
        [Route("proposedIngredient/{id}")]
        public async Task<ActionResult> DeleteProposedIngredient([FromRoute]int id)
        {
            await ingredientService.DeleteProposedIngredient(id);
            return Ok();
        }
    }
}
