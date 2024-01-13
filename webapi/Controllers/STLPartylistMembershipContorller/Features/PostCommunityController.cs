using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using webapi.App.Aggregates.STLPartylistMembership;
using Microsoft.Extensions.Configuration;
using webapi.App.RequestModel.Feature;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.AppRecruiter;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class PostCommunityController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IPostCommunityRepository _repo;
        public PostCommunityController(IConfiguration config, IPostCommunityRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("postcommunity/list")]
        public async Task<IActionResult> LoadPostCommunityListAsync([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadPostCommunityListAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", PostCommunity = result.comm });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", PostCommunity = result.comm });
            return NotFound();
        }
        [HttpPost]
        [Route("communities/list")]
        public async Task<IActionResult> LoadCommunityListAsync([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadCommunityListAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Communities = result.comm });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Communities = result.comm });
            return NotFound();
        }
        [HttpPost]
        [Route("commentpostcommunity/list")]
        public async Task<IActionResult> LoadCommentPostCommunityListAsync([FromBody] FilterRequest request)
        {
            var result = await _repo.LoadCommentPostCommunityListAsync(request);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", PostCommunity = result.comment });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", PostCommunity = result.comment });
            return NotFound();
        }

        [HttpPost]
        [Route("postcommentcommunity/add")]
        public async Task<IActionResult> AddPosition([FromBody] PostCommentCommunity req)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.SendPostCommentAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.message, Content = req });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("postcommunity/reaction")]
        public async Task<IActionResult> ReactionPostCommunityAsync([FromBody] PostCommunityReaction req)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.ReactionPostCommunityAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.Message });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.Message });
            return NotFound();
        }
        [HttpPost]
        [Route("commentpostcommunity/reaction")]
        public async Task<IActionResult> ReactionCommentPostCommunityAsync([FromBody] CommentPostCommunityReaction req)
        {
            //var result = await _repo.AddPosition(position.JsonString);
            var result = await _repo.ReactionCommentPostCommunityAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", Message = result.Message });
            else if (result.result != Results.Null)
                return Ok(new { Status = "error", Message = result.Message });
            return NotFound();
        }
    }
}
