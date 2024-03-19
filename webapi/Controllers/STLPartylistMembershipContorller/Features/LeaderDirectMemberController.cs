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
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class LeaderDirectMemberController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILeaderDirectMemberRepository _repo;
        public LeaderDirectMemberController(IConfiguration config, ILeaderDirectMemberRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("siteleader/member")]
        public async Task<IActionResult> LoadMember([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadMember(request);   
            if (result.result == Results.Success)
                return Ok(result.member);
            return NotFound();
        }
        [HttpPost]
        [Route("resident/respondent1")]
        public async Task<IActionResult> LoadRespondent([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_Resident(request);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, respondent = result.member });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", respondent = result.member });
                //return Ok(result.member);
            return NotFound();
        }
        [HttpPost]
        [Route("siteleader/member1")]
        public async Task<IActionResult> LoadMember1([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadMember1(request);
            if (result.result == Results.Success)
                return Ok(result.member);
            return NotFound();
        }
        [HttpPost]
        [Route("siteleader/member/new")]
        public async Task<IActionResult> Task0a([FromBody] STLMembership membership)
        {
            var valresult = await validity(membership);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();

            var result = await _repo.DirectAddMember(membership);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content=membership });
            else if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("siteleader/member/edit")]
        public async Task<IActionResult> Task0b([FromBody] STLMembership membership)
        {
            var valresult = await validity(membership);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();

            var result = await _repo.DirectAddMember(membership,true);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = membership });
            else if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }


        [HttpPost]
        [Route("siteleader/member/promote")]
        public async Task<IActionResult> PromoteMember([FromBody] STLMembership request)
        {
            var result = await _repo.PromoteMembertoLeader(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            else if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message, Content = request });
            return NotFound();
        }

        [HttpPost]
        [Route("member/changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] RequiredChangePassword request)
        {
            var repoResult = await _repo.ChangePassowrd(request);
            if (repoResult.result == Results.Success)
            {
                return Ok(new { Status = "ok", Message = repoResult.message, OTP=repoResult.otp });
            }
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        private async Task<(Results result, string message)> validity(STLMembership request)
        {
            if (request == null)
                return (Results.Null, null);
            if (!request.ImageUrl.IsEmpty())
                return (Results.Success, null);
            if (request.Img.IsEmpty())
                return (Results.Failed, "Please select an image.");
            byte[] bytes = Convert.FromBase64String(request.Img.Str());
            if (bytes.Length == 0)
                return (Results.Failed, "Make sure selected image is invalid.");
            var res = await ImgService.SendAsync(bytes);
            bytes.Clear();
            if (res == null)
                return (Results.Failed, "Please contact to admin.");
            var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
            if (json["status"].Str() != "error")
            {
                //request.ImageUrl = json["url"].Str();
                request.ImageUrl = (json["url"].Str()).Replace(_config["Portforwarding:LOCAL"].Str(), _config["Portforwarding:URL"].Str());
                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
        }
    }
}
