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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            return Ok(await categoryService.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById([FromRoute] int id)
        {
            return Ok(await categoryService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add([FromBody] CategoryDto categoryDto)
        {
            return Ok(await categoryService.Add(categoryDto));
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] CategoryDto categoryDto)
        {
            await categoryService.Delete(categoryDto);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<CategoryDto>> Update([FromBody] CategoryDto categoryDto)
        {
            return Ok(await categoryService.Update(categoryDto));
        }
    }
}
