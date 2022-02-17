using EatThisAPI.Helpers;
using EatThisAPI.Helpers.Enums;
using EatThisAPI.Models;
using EatThisAPI.Models.DTOs;
using EatThisAPI.Repositories;
using EatThisAPI.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Services
{
    public interface IReportService
    {
        Task<int> Add(ReportDto reportDto);
        Task<List<ReportDto>> GetCurrentUserReports();
    }
    public class ReportService : IReportService
    {
        private IReportRepository reportRepository;
        private IUserHelper userHelper;
        private IValidator validator;

        public ReportService(IReportRepository reportRepository, IUserHelper userHelper, IValidator validator)
        {
            this.reportRepository = reportRepository;
            this.userHelper = userHelper;
            this.validator = validator;
        }
        
        public async Task<int> Add(ReportDto reportDto)
        {
            validator.IsObjectNull(reportDto);
            var userId = userHelper.GetCurrentUserId();
            var report = new Report
            {
                Title = reportDto.Title,
                Description = reportDto.Description,
                UserId = userId,
                ReportStatusId = (int)ReportStatusEnum.ReportStatus.Reported
            };

            int id = await reportRepository.AddReport(report);
            return id;
        }

       public async Task<List<ReportDto>> GetCurrentUserReports()
       {
            var userId = userHelper.GetCurrentUserId();
            var reports = await reportRepository.GetReportsByUserId(userId);
            var reportsDto = new List<ReportDto>();
            foreach(var report in reports)
            {
                reportsDto.Add(new ReportDto
                {
                    Id = report.Id,
                    ReportStatus = new ReportStatusDto 
                    { 
                        Id = report.ReportStatus.Id,
                        Name = report.ReportStatus.Name
                    },
                    Title = report.Title
                });
            }

            return reportsDto;
       }
    }
}
