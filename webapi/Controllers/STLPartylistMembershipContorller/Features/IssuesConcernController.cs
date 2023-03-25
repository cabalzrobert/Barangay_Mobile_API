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

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl/concern")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class IssuesConcernController: ControllerBase
    {
        private readonly IIssuesConcernRepository _supRepo;
        public IssuesConcernController(IIssuesConcernRepository supRepo)
        {
            _supRepo = supRepo;
        }
        [HttpPost]
        [Route("report/problem/new")]
        public async Task<IActionResult> Task01([FromBody] ReportAProblemRequest request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _supRepo.SendIssuesConcernAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content=request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }
        [HttpPost]
        [Route("report/problem/edit")]
        public async Task<IActionResult> Task02([FromBody] ReportAProblemRequest request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _supRepo.UpdateIssuesConcernAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message, Content=request });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("report/problem/process")]
        public async Task<IActionResult> Task03([FromBody] ReportAProblemRequest request)
        {
            var repoResult = await _supRepo.ProcessIssuesConcernAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message, Content = request });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("report/problem/closed")]
        public async Task<IActionResult> Task05([FromBody] ReportAProblemRequest request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _supRepo.ClosedIssuesConcernAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Message = repoResult.message, Content = request });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("report/history")]
        public async Task<IActionResult> Task04([FromBody] FilterRequest request)
        {
            var result = await _supRepo.LoadIssuesConcern(request);
            if (result.result == Results.Success)
                return Ok(result.concern);
            return NotFound();
        }
        [HttpPost]
        [Route("report/problem/attachment")]
        public async Task<IActionResult> Task06([FromBody] ReportAProblemRequest request)
        {
            var result = await _supRepo.LoadIssuesConcernAttachment(request);
            if (result.result == Results.Success)
                return Ok(result.concern);
            return NotFound();
        }

        private async Task<(Results result, string message)> validity(ReportAProblemRequest request)
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
                var attachment = request.Attachments[i];
                byte[] bytes = Convert.FromBase64String(attachment.Str());
                if (bytes.Length == 0)
                    return (Results.Failed, "Make sure selected image is valid.");

                var res = await ImgService.SendAsync(bytes);
                bytes.Clear();
                if (res == null)
                    return (Results.Failed, "Please contact to admin.");

                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(res);
                if (json["status"].Str() != "error")
                {
                    string url = json["url"].Str();
                    sb.Append($"<item LNK_URL=\"{ url }\" />");
                    request.Attachments[i] = url;
                }
                else return (Results.Failed, "Make sure selected image is valid.");
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
