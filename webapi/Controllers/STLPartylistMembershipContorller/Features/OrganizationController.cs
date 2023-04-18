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
    [Route("app/v1/stl/organization")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class OrganizationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IOrganizationRepository _repo;
        public OrganizationController(IConfiguration config, IOrganizationRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("history")]
        public async Task<IActionResult> Task0a([FromBody] Organization req)
        {
            var result = await _repo.LoadOrganization(req);
            if (result.result == Results.Success)
                return Ok(result.orgz);
            else if (result.result != Results.Null)
                return Ok(result.orgz);
            return NotFound();
        }
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Task0b([FromBody] Organization req)
        {
            var result = await _repo.OrganizationAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = req });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> Task0c([FromBody] Organization req)
        {
            var result = await _repo.LoadOrganizationHistory(req);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, orgz = result.orgz });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", orgz = result.orgz });
            return NotFound();
        }
    }
}