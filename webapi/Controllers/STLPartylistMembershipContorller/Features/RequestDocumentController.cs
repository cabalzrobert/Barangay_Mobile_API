using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistDashboard.Features;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using System.IO;
using System.Text;
using webapi.App.RequestModel.Feature;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class RequestDocumentController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRequestDocumentRepository _repo;
        public RequestDocumentController(IConfiguration config, IRequestDocumentRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("reqdoc")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadRequestDocument(request);
            if (result.result == Results.Success)
                return Ok(result.reqdoc);
            return NotFound();
        }

        [HttpPost]
        [Route("phonebook/contact")]
        public async Task<IActionResult> Task011([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadContact(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", contact = result.contact });
            else if(result.result != Results.Null)
                return Ok(new { Status = "error", contact = result.contact });
            return NotFound();
        }


        [HttpPost]
        [Route("bimss/sendinvite")]
        public async Task<IActionResult> Task012([FromBody] FilterRequest request)
        {
            var result = await _repo.SendInvitation(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", message = result.message });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("doctype")]
        public async Task<IActionResult> Task08()
        {
            var result = await _repo.LoadDocumentType();
            
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, doctype = result.doctype });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", doctype = result.doctype });
            return NotFound();
        }

        [HttpPost]
        [Route("reqdoc/new")]
        public async Task<IActionResult> Task02([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.RequestDocumentAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content = request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("reqdoc/edit")]
        public async Task<IActionResult> Task03([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.UpdateRequestDocumentAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content = request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("reqclearance/new")]
        public async Task<IActionResult> Task04([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.RequestBrgyClearanceAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content = request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("reqclearance/edit")]
        public async Task<IActionResult> Task05([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.UpdateRequestBrgyClearanceAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content = request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("reqdoc/loadreqdocattm")]
        public async Task<IActionResult> Task07([FromBody] RequestDocument request)
        {
            var result = await _repo.LoadIssuesConcernAttachment(request);
            if (result.result == Results.Success)
                return Ok(result.reqdoc);
            return NotFound();
        }

        [HttpPost]
        [Route("reqdoc/otr/new")]
        public async Task<IActionResult> Task09([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.RequestBrgyDocumentAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content = request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("reqdoc/otr/edit")]
        public async Task<IActionResult> Task10([FromBody] RequestDocument request)
        {
            var valResult = await validity(request);
            if (valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if (valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _repo.UpdateRequestBrgyDocumentAsync(request);
            if (repoResult.result == Results.Success)
                return Ok(new { Status = "ok", Content = request, Message = repoResult.message });
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }

        private async Task<(Results result, string message)> validity(RequestDocument request)
        {
            if (request == null)
                return (Results.Null, null);

            if (request.Attachments == null || request.Attachments.Count < 1)
                return (Results.Success, null);
            StringBuilder sb = new StringBuilder();
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
                    //string url = json["url"].Str();
                    string url = (json["url"].Str()).Replace(_config["Portforwarding:LOCAL"].Str(), _config["Portforwarding:URL"].Str());
                    sb.Append($"<item LNK_URL=\"{ url }\" />");
                    request.Attachments[i] = url;
                }
                else return (Results.Failed, "Make sure selected image is valid.");
            }
            if (sb.Length > 0)
            {
                request.iAttachments = sb.ToString();
                return (Results.Success, null);
            }
            return (Results.Failed, "Make sure selected image is valid.");
        }
    }
}
