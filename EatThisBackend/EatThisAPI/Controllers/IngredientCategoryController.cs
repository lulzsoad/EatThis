using EatThisAPI.Models.DTOs;
using EatThisAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class IngredientCategoryController : ControllerBase
    {
        private readonly IIngredientCategoryService ingredientCategoryService;
        public IngredientCategoryController(IIngredientCategoryService ingredientCategoryService)
        {
            this.ingredientCategoryService = ingredientCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IngredientCategoryDto>>> GetAll()
        {
            return Ok(await ingredientCategoryService.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IngredientCategoryDto>> GetById([FromRoute] int id)
        {
            return Ok(await ingredientCategoryService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add([FromBody] IngredientCategoryDto ingredientCategoryDto)
        {
            return Ok(await ingredientCategoryService.Add(ingredientCategoryDto));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] IngredientCategoryDto ingredientCategoryDto)
        {
            await ingredientCategoryService.Delete(ingredientCategoryDto);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<IngredientCategoryDto>> Update([FromBody] IngredientCategoryDto ingredientCategoryDto)
        {
            return Ok(await ingredientCategoryService.Update(ingredientCategoryDto));
        }
    }
}
