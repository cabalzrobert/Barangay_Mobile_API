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
using Comm.Commons.Extensions;
using webapi.App.RequestModel.Common;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl/validid")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class GovernmentValidIDControllercs:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IGovernmentValidIDRepository _repo;
        public GovernmentValidIDControllercs(IConfiguration config, IGovernmentValidIDRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> Task0a([FromBody] Government_Valid_ID req)
        {
            var result = await _repo.LoadGovermentValidIDList(req);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, govid = result.govvalid });
            else if (result.result != Results.Null)
                return Ok(new {Status = result.result, govid = result.govvalid });
            return NotFound();
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Task0b([FromBody] Government_Valid_ID req)
        {
            var valresult = await validity(req);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();

            var result = await _repo.GovermentValidIDAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = req });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("history")]
        public async Task<IActionResult> Task0c([FromBody] Government_Valid_ID req)
        {
            var result = await _repo.LoadGovermentValidIDListHistory(req);
            if (result.result == Results.Success)
                return Ok(result.govvalid);
            else if (result.result != Results.Null)
                return Ok(result.govvalid);
            return NotFound();
        }

        private async Task<(Results result, string message)> validity(Government_Valid_ID request)
        {
            if (request == null)
                return (Results.Null, null);
            if (!request.GovValIDURL.IsEmpty())
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
                request.GovValIDURL = json["url"].Str();
                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
        }
    }
}
