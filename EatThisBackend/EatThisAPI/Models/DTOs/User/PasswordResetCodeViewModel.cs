using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs.User
{
    public class PasswordResetCodeViewModel
    {
        public string Email { get; set; }
        public string SecuredRoute { get; set; }
        public string Code { get; set; }
    }
}
