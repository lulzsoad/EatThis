using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs
{
    public class StepDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }
    }
}
