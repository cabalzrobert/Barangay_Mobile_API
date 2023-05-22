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
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    public class SitioController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ISitioRepository _repo;
        private readonly IAccountRepository _loginrepo;
        public SitioController(IConfiguration config, ISitioRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }
        [HttpPost]
        [Route("sitio")]
        public async Task<IActionResult> Sitio([FromBody] Sitio sit)
        {
            var result = await _repo.LoadSitio(sit);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, sitio = result.sit });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", sitio=result.sit });
            return NotFound();
        }
        [HttpPost]
        [Route("brgy")]
        public async Task<IActionResult> Brgy([FromBody] Barangay brgy)
        {
            var result = await _repo.LoadBarangay(brgy);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, Barangay = result.brgy });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Barangay = result.brgy });
            return NotFound();
        }
    }
}
