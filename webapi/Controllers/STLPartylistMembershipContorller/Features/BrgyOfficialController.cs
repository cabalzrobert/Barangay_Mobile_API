using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Aggregates.FeatureAggregate;
using webapi.App.RequestModel.Feature;
using webapi.App.Features.UserFeature;
using Comm.Commons.Extensions;
using Newtonsoft.Json;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using System;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl/official")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BrgyOfficialController:ControllerBase
    {
        private readonly IBrgyOfficialRepository _repo;
        public BrgyOfficialController(IBrgyOfficialRepository repo)
        {
            _repo = repo;
        }
        [HttpPost]
        [Route("brgyofficial/history")]
        public async Task<IActionResult> Task04([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_BrgyOfficial(request);
            if (result.result == Results.Success)
                return Ok(result.brgyofficial);
            return NotFound();
        }
    }
}
