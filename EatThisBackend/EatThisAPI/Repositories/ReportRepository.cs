using EatThisAPI.Database;
using EatThisAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Repositories
{
    public interface IReportRepository
    {
        Task<int> AddReport(Report report);
        Task<List<Report>> GetReportsByUserId(int userId);
    }
    public class ReportRepository : IReportRepository
    {
        private AppDbContext context;
        public ReportRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<int> AddReport(Report report)
        {
            context.Reports.Add(report);
            await context.SaveChangesAsync();
            return report.Id; 
        }

        public async Task<List<Report>> GetReportsByUserId(int userId)
        {
            return await context.Reports.AsNoTracking()
                .Include(x => x.ReportStatus)
                .Where(x => x.UserId == userId)
                .Select(x => new Report
                {
                    Id = x.Id,
                    Title = x.Title,
                    ReportStatus = x.ReportStatus
                })
                .ToListAsync();
        }
    }
}
