using EatThisAPI.Models.DTOs.User;
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
    [Route("api/[Controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            await accountService.RegisterUser(registerUserDto);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            return Ok(await accountService.GenerateJwtToken(loginDto));
        }

        [HttpGet]
        [Route("activate/{activationCode}")]
        public async Task<ActionResult> Activate([FromRoute] string activationCode)
        {
            return Ok(await accountService.CheckAndActivateAccount(activationCode));
        }

        [HttpGet]
        [Route("forgotten-password")]
        public async Task<ActionResult> GeneratePasswordResetCode([FromQuery] string email)
        {
            return Ok(await accountService.GeneratePasswordResetCode(email));
        }

        [HttpPost]
        [Route("check-reset-code")]
        public async Task<ActionResult> CheckPasswordResetCode([FromBody] PasswordResetCodeViewModel passwordResetCodeModel)
        {
            return Ok(await accountService.PasswordResetCodeCheck(passwordResetCodeModel));
        }

        [HttpPost]
        [Route("change-password-reset-code")]
        public async Task<ActionResult> ChangePasswordResetCode([FromBody] ChangePasswordResetCodeViewModel changePasswordResetCodeModel)
        {
            await accountService.ChangePasswordByResetCode(changePasswordResetCodeModel);
            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("change-password")]
        public async Task<ActionResult> ChangeCurrentUserPassword([FromBody] ChangePasswordViewModel changePasswordVM)
        {
            await accountService.ChangePassword(changePasswordVM);
            return Ok();
        }
    }
}
