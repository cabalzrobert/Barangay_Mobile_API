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
    [Route("app/v1/stl/educ")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class EducationAttainmentController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IEducationAttainmentRepository _repo;
        public EducationAttainmentController(IConfiguration config, IEducationAttainmentRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("loadattainment")]
        public async Task<IActionResult> Task0a()
        {
            var result = await _repo.LoadEducationalAttainment();
            if (result.result == Results.Success)
                return Ok(result.educattainment );
            else if (result.result != Results.Null)
                return Ok(result.educattainment);
            return NotFound();
        }
        [HttpPost]
        [Route("loadattainment/update")]
        public async Task<IActionResult> Task0b([FromBody] EducAttainment grp)
        {
            var result = await _repo.EducationalAttainmentAsync(grp);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = grp });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
    }
}
