using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BlockedUserAccountController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBlockedUserAccountRepository _repo;
        public BlockedUserAccountController(IConfiguration config, IBlockedUserAccountRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("useracount/blocked")]
        public async Task<IActionResult> Blocked_Account([FromBody] BlockedUserAccount request)
        {
            var result = await _repo.BlockedUserAccountAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("useracount/unblocked")]
        public async Task<IActionResult> UnBlocked_Account([FromBody] BlockedUserAccount request)
        {
            var result = await _repo.UnBlockedUserAccountAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("useracount/blocked/list")]
        public async Task<IActionResult> Blocked_Account_List([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadBlockedUserAccount(request);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, blockedaccount = result.blockedaccount });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", blockedaccount = result.blockedaccount });
            return NotFound();
        }
    }
}
