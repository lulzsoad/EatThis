using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class UserActivatingCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ActivatingCode { get; set; }

        public virtual User User { get; set; }
    }
}
