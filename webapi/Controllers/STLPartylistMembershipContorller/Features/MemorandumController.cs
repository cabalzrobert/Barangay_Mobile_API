using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class MemorandumController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMemorandumRepository _repo;
        public MemorandumController(IConfiguration config, IMemorandumRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("memo")]
        public async Task<IActionResult> Task04([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadMemorandum(request);
            if (result.result == Results.Success)
                return Ok(result.memo);
            return NotFound();
        }
    }
}
