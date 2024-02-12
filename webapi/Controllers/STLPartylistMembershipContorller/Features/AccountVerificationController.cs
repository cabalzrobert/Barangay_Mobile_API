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
    [Route("app/v1/stl/account")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class AccountVerificationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _loginrepo;
        private readonly IAccountVerificationRepository _repo;
        public AccountVerificationController(IConfiguration config, IAccountVerificationRepository repo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _config = config;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("verification")]
        public async Task<IActionResult> SendAccountVerification([FromBody] AccountVerification request)
        {
            var valresult = await validity(request);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();

            var results = await _repo.SendAccountVerification(request);
            if(results.result == Results.Success)
                return Ok(new { status = "ok", message = results.message });
            return Ok(new { status = "failed", message = results.message });
        }

        private async Task<(Results result, string message)> validity(AccountVerification request)
        {
            if (request == null)
                return (Results.Null, null);
            if (!request.SelfieUrl.IsEmpty())
                return (Results.Success, "Please select an image.");
            if (!request.DocUrl.IsEmpty())
                return (Results.Success, "Please select an image.");

            //if (request.Img.IsEmpty())
            //    return (Results.Failed, "Please select an image.");
            byte[] bytes1 = Convert.FromBase64String(request.SelfieUrl.Str());
            byte[] bytes2 = Convert.FromBase64String(request.DocUrl.Str());
            if (bytes1.Length == 0 || bytes2.Length == 0)
                return (Results.Failed, "Make sure selected image is invalid.");
            var res1 = await ImgService.SendAsync(bytes1);
            var res2 = await ImgService.SendAsync(bytes2);
            bytes1.Clear();
            bytes2.Clear();
            if (res1 == null || res2 == null)
                return (Results.Failed, "Please contact to admin.");
            var json1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(res1);
            var json2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(res2);
            if (json1["status"].Str() != "error" || json2["status"].Str() != "error")
            {
                //request.ImageUrl = json["url"].Str();
                request.SelfieUrl = (json1["url"].Str()).Replace(_config["Portforwarding:LOCAL"].Str(), _config["Portforwarding:URL"].Str());
                request.DocUrl = (json2["url"].Str()).Replace(_config["Portforwarding:LOCAL"].Str(), _config["Portforwarding:URL"].Str());
                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
        }


    }
}
