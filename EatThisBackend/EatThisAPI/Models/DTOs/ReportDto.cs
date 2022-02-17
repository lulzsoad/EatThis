using EatThisAPI.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models.DTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ReportStatusDto ReportStatus { get; set; }
        public UserDetails User { get; set; }
    }
}
