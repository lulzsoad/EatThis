using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.User;
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
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById([FromRoute] int userId)
        {
            return Ok(await userService.GetUserById(userId));
        }

        [Authorize]
        [HttpGet]
        [Route("currentUser")]
        public async Task<ActionResult<UserDetails>> GetCurrentUserDetails()
        {
            return Ok(await userService.GetCurrentUserDetails());
        }
    }
}
