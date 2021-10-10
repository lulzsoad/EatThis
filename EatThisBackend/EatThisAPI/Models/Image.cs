using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public virtual Step Step { get; set; }
    }
}
