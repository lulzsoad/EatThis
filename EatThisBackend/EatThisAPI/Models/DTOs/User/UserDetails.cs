using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs.User
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int RoleId { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string FullName { get => FirstName + " " + LastName; }
    }
}
