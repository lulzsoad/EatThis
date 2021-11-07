using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs.User
{
    public class ChangePasswordResetCodeViewModel
    {
        public PasswordResetCodeViewModel PasswordResetCode { get; set; }
        public string Password { get; set; }
    }
}
