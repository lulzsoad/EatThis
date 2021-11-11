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
    public class UnitController : ControllerBase
    {
        private readonly IUnitService unitService;
        public UnitController(IUnitService unitService)
        {
            this.unitService = unitService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitDto>>> GetAll()
        {
            return Ok(await unitService.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UnitDto>> GetById([FromRoute] int id)
        {
            return Ok(await unitService.GetById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<int>> Add([FromBody] UnitDto unitDto)
        {
            return Ok(await unitService.Add(unitDto));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> Delete([FromBody] UnitDto unitDto)
        {
            await unitService.Delete(unitDto);
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<UnitDto>> Update([FromBody] UnitDto unitDto)
        {
            return Ok(await unitService.Update(unitDto));
        }
    }
}
