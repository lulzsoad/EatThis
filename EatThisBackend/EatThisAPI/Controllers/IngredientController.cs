using EatThisAPI.Models.DTOs;
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
    }
}
