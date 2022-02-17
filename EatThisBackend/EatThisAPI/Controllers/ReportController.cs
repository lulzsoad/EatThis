using EatThisAPI.Models.DTOs;
using EatThisAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EatThisAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReportController : ControllerBase
    {
        private IReportService reportService;
        public ReportController(IReportService reportService)
        {
            this.reportService = reportService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddReport([FromBody]ReportDto reportDto)
        {
            return Ok(await reportService.Add(reportDto));
        }

        [Authorize]
        [HttpGet]
        [Route("currentUser")]
        public async Task<ActionResult> GetCurrentUserReports()
        {
            return Ok(await reportService.GetCurrentUserReports());
        }
    }
}
