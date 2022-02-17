using EatThisAPI.Models;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EatThisAPI.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetCurrentUser();
        int GetCurrentUserId();
    }
    public class UserHelper : ControllerBase, IUserHelper
    {
        private IHttpContextAccessor httpContextAccessor;
        private IUserRepository userRepository;
        private IUserValidator userValidator;

        public UserHelper(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IUserValidator userValidator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.userValidator = userValidator;
        }

        public async Task<User> GetCurrentUser()
        {
            var userClaim = httpContextAccessor.HttpContext?.User;
            var userId = userClaim.FindFirst(ClaimTypes.NameIdentifier).Value;
            int id = Convert.ToInt32(userId);
            var user = await userRepository.GetUserById(id);
            userValidator.UserExists(user);
            return user;
        }

        public int GetCurrentUserId()
        {
            var userClaim = httpContextAccessor.HttpContext?.User;
            var userId = userClaim.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Convert.ToInt32(userId);
        }
    }
}
