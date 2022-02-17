using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReportStatusId { get; set; }
        public ReportStatus ReportStatus { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
