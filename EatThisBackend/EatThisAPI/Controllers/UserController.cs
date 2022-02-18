using EatThisAPI.Models.DTOs;
using EatThisAPI.Models.DTOs.User;
using EatThisAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        [Authorize]
        [HttpPatch]
        [Route("currentUser")]
        public async Task<ActionResult<UserDetails>> PatchCurrentUser([FromBody] JsonPatchDocument userDetails)
        {
            return Ok(await userService.UpdateCurrentUser(userDetails));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetChunkOfUsers([FromQuery] int skip, [FromQuery] int take, [FromQuery] string search)
        {
            return Ok(await userService.GetChunkOfUsers(skip, take, search));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("changeUserRole")]
        public async Task<ActionResult> ChangeUserRole([FromQuery] int userId, [FromBody] RoleDto role)
        {
            await userService.ChangeUserRole(userId, role);
            return Ok();
        }
    }
}
