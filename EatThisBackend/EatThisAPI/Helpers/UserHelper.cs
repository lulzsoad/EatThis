using EatThisAPI.Models;
using EatThisAPI.Repositories;
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
    }
    public class UserHelper : ControllerBase, IUserHelper
    {
        private IHttpContextAccessor httpContextAccessor;
        private IUserRepository userRepository;

        public UserHelper(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public async Task<User> GetCurrentUser()
        {
            var userClaim = httpContextAccessor.HttpContext?.User;
            var userId = userClaim.FindFirst(ClaimTypes.NameIdentifier).Value;
            int id = Convert.ToInt32(userId);
            var user = await userRepository.GetUserById(id);
            return user;
        }
    }
}
