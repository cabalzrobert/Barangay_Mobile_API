using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class DonationsController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDonationsRepository _repo;
        public DonationsController(IConfiguration config, IDonationsRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("donation")]
        public async Task<IActionResult> LoadDonation([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadDonations(request);
            if (result.result == Results.Success)
                return Ok(result.donation);
            return NotFound();
        }
        [HttpPost]
        [Route("claimdonation")]
        public async Task<IActionResult> LoadClaimDonation([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadClaimDonations(request);
            if (result.result == Results.Success)
                return Ok(result.donation);
            return NotFound();
        }
        [HttpPost]
        [Route("donation/claim")]
        public async Task<IActionResult> Task0d([FromBody] FilterRequest request)
        {
            var result = await _repo.ClaimDonation(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
    }
}
