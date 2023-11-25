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
using webapi.App.RequestModel.Feature;
using webapi.App.Aggregates.FeatureAggregate;
using MySqlX.XDevAPI.Common;

namespace webapi.Controllers.STLPartylistDashboardContorller.Features
{
    [Route("app/v1/stl/connection")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class ConnectionRequestController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _loginrepo;
        private readonly IConnectionRequestRepository _repo;
        private readonly IMessengerAppRepository _chatrepo;
        public ConnectionRequestController(IConfiguration config, IConnectionRequestRepository repo, IMessengerAppRepository chatrepo, IAccountRepository loginrepo)
        {
            _repo = repo;
            _chatrepo = chatrepo;
            _config = config;
            _loginrepo = loginrepo;
        }

        [HttpPost]
        [Route("request/list")]
        public async Task<IActionResult> GetConnectionRequestList(int segment)
        {
            var result = await _repo.GetConnectionRequestList(segment);
            if (result.result == Results.Success)
                return Ok(result.list);
            return Ok(null);
        }

        [HttpPost]
        [Route("request/update")]
        public async Task<IActionResult> UpdateConnectionRequest([FromBody] ConnectionRequest req)
        {
            var result = await _repo.UpdateConnectionRequest(req);
            if (result.result == Results.Success)
            {
                if (req.IsAccepted) { 
                    var result1 = await _chatrepo.RequestPersonalChatAsync(result.reqid, req.ConnectionRequestId);
                    return Ok(new { Status = "ok", Message = result.message, ChatInfo = result1.item });
                }
                return Ok(new { Status = "ok", Message = result.message });
            }
            else if (result.result == Results.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }


        //[HttpPost]
        //[Route("residents/search")]
        //public async Task<IActionResult> GetListOfSearchName([FromBody] Resident req)
        //{
        //    var result = await _repo.SearchedResidents(req);

        //    if (result.result == Results.Success)
        //        return Ok(result.searched);
        //    else if (result.result == Results.Failed)
        //        return Ok(null);
        //    return NotFound();
        //}

        //[HttpPost]
        //[Route("family/add")]
        //public async Task<IActionResult> AddFamily([FromBody] Family req)
        //{
        //    var result = await _repo.AddFamily(req);
        //    if (result.result == Results.Success)
        //        return Ok(new { Status = "ok", Message = result.message, FamId = result.famid });
        //    else if (result.result == Results.Failed)
        //        return Ok(new { Status = "error", Message = result.message });
        //    return NotFound();
        //}

        //[HttpPost]
        //[Route("family/update")]
        //public async Task<IActionResult> EditFamily([FromBody] Family req)
        //{
        //    var result = await _repo.EditDeleteFamily(req);
        //    if (result.result == Results.Success)
        //        return Ok(new { Status = "ok", Message = result.message });
        //    else if (result.result == Results.Failed)
        //        return Ok(new { Status = "error", Message = result.message });
        //    return NotFound();
        //}
    }
}
