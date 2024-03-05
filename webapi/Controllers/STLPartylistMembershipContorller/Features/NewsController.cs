using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistMembership.Features;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistMembershipContorller.Features
{
    [Route("app/v1/stl/news")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class NewsController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly INewsRepository _repo;
        public NewsController(IConfiguration config, INewsRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("list")]
        public async Task<IActionResult> Task01([FromBody] FilterRequest request)
        {
            var result = await _repo.Load_NewsAsync(request);
            if (result.result == Results.Success)
                return Ok(result.item);
            return NotFound();
        }
        [HttpPost]
        [Route("category")]
        public async Task<IActionResult> category()
        {
            var result = await _repo.Load_CategoryAsync();
            if (result.result == Results.Success)
                return Ok(result.item);
            return NotFound();
        }
    }
}
