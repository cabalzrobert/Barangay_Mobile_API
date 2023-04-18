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
using Comm.Commons.Extensions;
using webapi.App.RequestModel.Common;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class EstablishmentController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IEstablishmentRespository _repo;
        public EstablishmentController(IConfiguration config, IEstablishmentRespository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("establishment/list")]
        public async Task<IActionResult> Task0a(FilterRequest req)
        {
            var result = await _repo.Load_Establishment(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", establishment = result.est });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", establishment = result.est });
            return NotFound();
        }
    }
}
