using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class StepImage
    {
        [ForeignKey("Step")]
        public int Id { get; set; }
        public string Url { get; set; }

        public virtual Step Step { get; set; }
    }
}
