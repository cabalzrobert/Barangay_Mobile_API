using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;
using webapi.App.Aggregates.STLPartylistMembership;
using webapi.App.Model.User;
using System.Security.Claims;
using Comm.Commons.Advance;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using webapi.App.Aggregates.Common.Dto;
using System.Text;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistMembershipContorller
{
    [Route("app/v1/stl/brgyoperator")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BrgyOperatorController:ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly IBrgyOperatorRepository _repo;
        public BrgyOperatorController(IConfiguration config, IBrgyOperatorRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> Task0d([FromBody] FilterRequest req)
        {
            var result = await _repo.Load_BrgyOperator(req);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, brgyoptr = result.bryoptr });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", brgyoptr = result.bryoptr });
            return NotFound();
        }

        [HttpPost]
        [Route("prevaccount")]
        public async Task<IActionResult> Task0e([FromBody] FilterRequest req)
        {
            var result = await _repo.Load_PrevComAccount(req);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, brgyoptr = result.prev });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", brgyoptr = result.prev });
            return NotFound();
        }
    }
}
