using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual UserActivatingCode UserActivatingCode { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
