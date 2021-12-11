using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs.User
{
    public class AuthToken
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        
        public string Token { get; set; }
        public DateTime TokenExpirationDate { get; set; }
    }
}
