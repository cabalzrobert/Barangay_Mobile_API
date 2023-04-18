using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl/employment")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class EmploymentHistoryController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IEmploymentHistoryRepository _repo;
        public EmploymentHistoryController(IConfiguration config, IEmploymentHistoryRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("history")]
        public async Task<IActionResult> Task0a()
        {
            var result = await _repo.LoadEmploymentHistory();
            if (result.result == Results.Success)
                return Ok(result.emphistory);
            else if (result.result != Results.Null)
                return Ok(result.emphistory);
            return NotFound();
        }
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Task0b([FromBody] Employment_History req)
        {
            var result = await _repo.EmploymentHistoryAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = req });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
    }
}
