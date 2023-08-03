using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Aggregates.FeatureAggregate;
using webapi.App.RequestModel.Feature;
using webapi.App.Features.UserFeature;
using Comm.Commons.Extensions;
using Newtonsoft.Json;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using System;
using webapi.App.RequestModel.Common;
using Microsoft.Extensions.Configuration;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl/blotter")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BlotterController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBlotterRepository _supRepo;
        public BlotterController(IBlotterRepository supRepo)
        {
            _supRepo = supRepo;
        }
        [HttpPost]
        [Route("complaint/new")]
        public async Task<IActionResult> Task01([FromBody] ComplaintBlotter request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _supRepo.ComplaintAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content = request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("complaint/edit")]
        public async Task<IActionResult> Task02([FromBody] ComplaintBlotter request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _supRepo.ComplaintAsync(request, true);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content = request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("complaint/history")]
        public async Task<IActionResult> Task04([FromBody] ComplaintBlotter request)
        {
            var result = await _supRepo.LoadComplaint(request);
            if (result.result == Results.Success)
                return Ok(result.blotter);
            return NotFound();
        }
        [HttpPost]
        [Route("respondent/history")]
        public async Task<IActionResult> Task05([FromBody] ComplaintBlotter request)
        {
            var result = await _supRepo.LoadRespondent(request);
            if (result.result == Results.Success)
                return Ok(result.blotter);
            return NotFound();
        }
        [HttpPost]
        [Route("complaint/attachment")]
        public async Task<IActionResult> Task06([FromBody] ComplaintBlotter request)
        {
            var result = await _supRepo.LoadComplaintAttachment(request);
            if (result.result == Results.Success)
                return Ok(result.blotter);
            return NotFound();
        }

        private async Task<(Results result, string message)> validity(ComplaintBlotter request)
        {
            if (request == null)
                return (Results.Null, null);
            //if(!request.ImageUrl.IsEmpty())
            //    return (Results.Success, null);

            if (request.Attachments == null || request.Attachments.Count < 1)
                return (Results.Success, null);
            //var attachments = request.Attachments;
            StringBuilder sb = new StringBuilder();
            //request.iAttachments = "";
            for (int i = 0; i < request.Attachments.Count; i++)
            {
                var attachment = request.Attachments[i].Str();
                if (attachment.IsEmpty()) continue;
                if (attachment.StartsWith("http"))
                {
                    sb.Append($"<item LNK_URL=\"{attachment}\" />");
                }
                else
                {
                    byte[] bytes = Convert.FromBase64String(attachment);
                    if (bytes.Length == 0)
                        return (Results.Failed, "Make sure selected image is valid.");

                    var res = await ImgService.SendAsync(bytes);
                    bytes.Clear();
                    if (res == null)
                        return (Results.Failed, "Please contact to admin.");

                    var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                    if (json["status"].Str() != "error")
                    {
                        //string url = json["url"].Str();
                        string url = (json["url"].Str()).Replace(_config["Portforwarding:LOCAL"].Str(), _config["Portforwarding:URL"].Str());
                        sb.Append($"<item CASENO_URL=\"{ url }\" />");
                        request.Attachments[i] = url;
                    }
                    else return (Results.Failed, "Make sure selected image is valid.");
                }
                
            }
            if (sb.Length > 0)
            {
                request.iAttachments = sb.ToString();
                //request.Attachments = null;
                return (Results.Success, null);
            }
            return (Results.Failed, "Make sure selected image is valid.");
        }
    }
}
