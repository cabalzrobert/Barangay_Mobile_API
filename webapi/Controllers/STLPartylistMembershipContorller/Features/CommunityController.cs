using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using webapi.App.Aggregates.STLPartylistMembership;
using Microsoft.Extensions.Configuration;
using webapi.App.RequestModel.Feature;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.AppRecruiter;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class CommunityController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ICommunityRepository _repo;
        public CommunityController(IConfiguration config, ICommunityRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("community/list")]
        public async Task<IActionResult> Blocked_Account_List([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadCommunityListAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, Community = result.comm });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Community = result.comm });
            return NotFound();
        }
        [HttpPost]
        [Route("community/join")]
        public async Task<IActionResult> SendRequestJoinCommunityAsync([FromBody] Community request)
        {
            var result = await _repo.SendRequestJoinCommunityAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("community/leave")]
        public async Task<IActionResult> LeaveCommunityAsync([FromBody] PostCommentCommunity request)
        {
            var result = await _repo.LeaveCommunityAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.Message });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.Message });
            return NotFound();
        }
        [HttpPost]
        [Route("community/count")]
        public async Task<IActionResult> GetCountCommunityAsync()
        {
            var result = await _repo.GetCountCommunityAsync();
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", CommunityCount = result.CountCommunity });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", CommunityCount = result.CountCommunity });
            return NotFound();
        }
    }
}
