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

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class GroupController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IGroupRepository _repo;
        public GroupController(IConfiguration config, IGroupRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("group/siteleader")]
        public async Task<IActionResult> SiteLeader([FromBody] Group grp)
        {
            var result = await _repo.SiteLeader(grp);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, siteleader = result.ldr });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", siteleader = result.ldr });
            return NotFound();
        }
    }
}
