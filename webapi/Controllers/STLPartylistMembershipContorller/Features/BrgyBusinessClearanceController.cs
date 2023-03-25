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
    public class BrgyBusinessClearanceController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBrgyBusinessClearanceRepository _repo;
        public BrgyBusinessClearanceController(IConfiguration config, IBrgyBusinessClearanceRepository repo) 
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost]
        [Route("reqbrgybusinessclearance")]
        public async Task<IActionResult> Task01([FromBody] BrgyBusinessClearance request)
        {
            var result = await _repo.Load_BrgyClearance(request);
            if (result.result == Results.Success)
                return Ok(result.brybizclrid);
            return NotFound();
        }

        [HttpPost]
        [Route("reqbrgybusinessclearance/request")]
        public async Task<IActionResult> RequestBarangayBusinessClearance([FromBody] BrgyBusinessClearance request)
        {
            var result = await _repo.RequstBrgyBusinessClearanceAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("business")]
        public async Task<IActionResult> Business([FromBody] FilterRequest request)
        {
            var result = await _repo.LaodBusiness(request);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, biz = result.biz });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", biz = result.biz });
            return NotFound();
        }
    }
}
