using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppInformationController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAppInformation()
        {
            return Ok(new {Status = "Authorized" });
        }
    }
}
