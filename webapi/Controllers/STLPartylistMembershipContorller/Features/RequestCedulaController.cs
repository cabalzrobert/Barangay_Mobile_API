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
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using webapi.App.RequestModel.Feature;

namespace webapi.Controllers.STLPartylistMembership.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    //[ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class RequestCedulaController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly IRequestCedulaRepository _repo;
        private readonly IAccountRepository _loginrepo;
        public RequestCedulaController(IConfiguration config, IRequestCedulaRepository repo, IAccountRepository loginrepo)
        {
            _config = config;
            _repo = repo;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("cedula/new")]
        public async Task<IActionResult> Request([FromBody] CedulaRequest request)
        {
            var reporesult = await _repo.Request(request);

            if (reporesult.result == Results.Success)
                return Ok(new { Status = "ok", Message = reporesult.message});
            else if (reporesult.result == Results.Failed)
                return Ok(new { Status = "error", Message = reporesult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("cedula/cancel")]
        public async Task<IActionResult> Cancel([FromBody] CedulaRequest request)
        {
            var reporesult = await _repo.Cancel(request);

            if (reporesult.result == Results.Success)
                return Ok(new { Status = "ok", Message = reporesult.message });
            else if (reporesult.result == Results.Failed)
                return Ok(new { Status = "error", Message = reporesult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("cedula/list")]
        public async Task<IActionResult> Load([FromBody] CedulaRequest request)
        {
            var result = await _repo.Load(request);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, requestcedula = result.list });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", requestcedula = result.list });
            return NotFound();
        }

        [HttpPost]
        [Route("cedula/series")]
        public async Task<IActionResult> GenerateSeries([FromBody] CedulaRequest request)
        {
            var result = await _repo.GenerateSeries(request);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, series = result.series });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", series = result.series });
            return NotFound();
        }
    }
}
