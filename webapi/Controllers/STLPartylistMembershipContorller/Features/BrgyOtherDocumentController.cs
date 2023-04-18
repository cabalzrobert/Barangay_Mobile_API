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


namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl/otherdocument")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class BrgyOtherDocumentController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IBrgyOtherDocumentRepository _repo;
        public  BrgyOtherDocumentController(IConfiguration config, IBrgyOtherDocumentRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("history")]
        public async Task<IActionResult> Task01([FromBody] LegalDocument_Transaction request)
        {
            var result = await _repo.Load_OtherDocumentRequest(request);
            if (result.result == Results.Success)
                return Ok(result.lgldoctrans);
            return NotFound();
        }

        [HttpPost]
        [Route("request/new")]
        public async Task<IActionResult> Task02([FromBody] LegalDocument_Transaction request)
        {
            var result = await _repo.RequestBrgyOtherDocumentAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }

        [HttpPost]
        [Route("request/edit")]
        public async Task<IActionResult> Task05([FromBody] LegalDocument_Transaction request)
        {
            var result = await _repo.RequestBrgyOtherDocumentAsync(request, true);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = request });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("templatetype")]
        public async Task<IActionResult> Task03()
        {
            var result = await _repo.Load_TemplateType();
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, templatetype = result.tpl });
            return NotFound();
        }


        [HttpPost]
        [Route("templatedoc")]
        public async Task<IActionResult> Task04(string templatetypeid)
        {
            var result = await _repo.Load_TemplateDoc(templatetypeid);
            if (result.result == Results.Success)
                return Ok(new { Status = result.result, templatedoc = result.tpldoc });
            return NotFound();
        }
    }
}
