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
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BrgyClearanceController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBrgyClearanceRepository _repo;
        public BrgyClearanceController(IConfiguration config, IBrgyClearanceRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("reqbrgyclearance")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_BrgyClearance(request);
            if (result.result == Results.Success)
                return Ok(result.bryclrid);
            return NotFound();
        }
        [HttpPost]
        [Route("reqbrgyclearance/request")]
        public async Task<IActionResult> LoadPurpose([FromBody] BrgyClearance request)
        {
            var result = await _repo.RequestBrgyClearanceAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content=request });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("certtype")]
        public async Task<IActionResult> TypeofClearance()
        {
            var result = await _repo.LoadCertificateType();
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, certtyp = result.certtyp });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", certtyp = result.certtyp });
            return NotFound();
        }

        [HttpPost]
        [Route("reqbrgyclearance/purpose")]
        public async Task<IActionResult> LoadPurpose()
        {
            var result = await _repo.LoadPurpose();
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, purpose = result.purpose });
            else if(result.result != Results.Null)
                return Ok(new { Status = "error", purpose = result.purpose });
            return NotFound();
        }
    }
}
