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
using webapi.Controllers.STLPartylistMembershipContorller.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stl/_profile")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class ProfileController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _loginrepo;
        private readonly IProfileRepository _repo;
        public ProfileController(IConfiguration config, IProfileRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("about")]
        public async Task<IActionResult> LoadProfile()
        {
            var workResult = await _repo.LoadEmploymentHistory();
            object work = null;
            if (workResult.result == Results.Success)
                work = workResult.emphistory;
            else
                work = workResult.emphistory;

            var eduResult = await _repo.LoadEducationalAttainment();
            object education = null;
            if (eduResult.result == Results.Success)
                education = eduResult.educattainment;
            else
                education = eduResult.educattainment;

            var govidResult = await _repo.LoadGovermentValidIDListHistory();
            object govid = null;
            if (govidResult.result == Results.Success)
                govid = govidResult.govvalid;
            else
                govid = govidResult.govvalid;

            var orgResult = await _repo.LoadOrganization();
            object organization = null;
            if (orgResult.result == Results.Success)
                organization = orgResult.orgz;
            else
                organization = orgResult.orgz;

            return Ok(new { work = work, education = education, govid = govid, organization = organization });
        }
    }
}
