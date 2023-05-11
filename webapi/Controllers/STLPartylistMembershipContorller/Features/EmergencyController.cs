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
using webapi.App.RequestModel.Feature;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class EmergencyController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IEmergencyRepository _repo;
        private readonly IAccountRepository _loginrepo;
        public EmergencyController(IConfiguration config, IEmergencyRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("emergency/type")]
        public async Task<IActionResult> EmergencyType([FromBody] EmergencyRequest request)
        {
            var result = await _repo.LoadEmergencyType(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", emergency= result.list });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", emergency= result.list });
            return NotFound();
        }

        [HttpPost]
        [Route("emergency/send")]
        public async Task<IActionResult> SendEmergencyAlert([FromBody] EmergencyRequest request)
        {
            var result = await _repo.SendEmergencyAlert(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }


        [HttpPost]
        [Route("emergency/list")]
        public async Task<IActionResult> EmergencyAlertList([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadEmergencyAlert(request);
            if (result.result == Results.Success)
                return Ok(result.list);
            return NotFound();
        }
    }
}
