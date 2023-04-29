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
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;

namespace webapi.Controllers.STLPartylistMembership.Features
{
    [Route("app/v1/stl")]
    [ApiController]
    //[ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class STLMembershipController:ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ISTLMembershipRepository _repo;
        private readonly IAccountRepository _loginrepo;
        public STLMembershipController(IConfiguration config,  ISTLMembershipRepository repo, IAccountRepository loginrepo)
        {
            _config = config;
            _repo = repo;
            _loginrepo = loginrepo;
        }
        [HttpPost]
        [Route("membership/new")]
        public async Task<IActionResult> Task0b([FromBody] ResidentsInfo request)
        {
            var valresult = await validity(request);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();
            var reporesult = await _repo.MembershipAsync(request);

            if (reporesult.result == Results.Success)
            {
                //var loginresult = await _loginrepo.STLSignInAsync(reporesult.signin);
                //if (loginresult.result == SignInResults.Success)
                //{
                //    var token = CreateToken(loginresult.account);
                //    var data = await _loginrepo.MemberGroup(loginresult.account);
                //    return Ok(new { Status = "ok", Account = loginresult.account, Auth = token, Company = data.PartyList, Group = data.Group });
                //}
                //return Ok(new { Status = "error", Message = loginresult.message, Content = request });
                return Ok(new { Status = "ok", Message = reporesult.message, Content=request });
            }
                
            else if (reporesult.result == Results.Failed)
                return Ok(new { Status = "error", Message = reporesult.message });
            return NotFound();
        }

        [HttpPost]
        [Route("membership/edit")]
        public async Task<IActionResult> Task0c([FromBody] ResidentsInfo request)
        {
            //Temporary comment
            var valresult = await validity(request);
            if (valresult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valresult.message });
            if (valresult.result != Results.Success)
                return NotFound();

            var reporesult = await _repo.UpdateMembershipAsync(request);
            if (reporesult.result == Results.Success)
                return Ok(new { Status = "ok", Message = reporesult.message });
            else if (reporesult.result == Results.Failed)
                return Ok(new { Status = "error", Message = reporesult.message });
            return NotFound();
        }

        //[HttpPost]
        //[Route("brgyoperator")]
        //public async Task<IActionResult> Task0d([FromBody] FilterRequest req)
        //{
        //    var result = await _repo.Load_BrgyOperator(req);
        //    if (result.result == Results.Success)
        //        return Ok(new { Status = result.result, brgyoptr = result.bryoptr });
        //    else if(result.result != Results.Null)
        //        return Ok(new { Status = "error", brgyoptr = result.bryoptr });
        //    return NotFound();
        //}

        private async Task<(Results result, string message)> validity(ResidentsInfo request)
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
                request.ImageUrl = json["url"].Str();
                return (Results.Success, null);
            }
            return (Results.Null, "Make sure selected image is invalid");
        }

        private object CreateToken(STLAccount user)
        {
            var guid = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim("token", Cipher.Encrypt(JsonConvert.SerializeObject(new{
                    GUID = Cipher.MD5Hash(guid),
                    PL_ID = user.PL_ID,
                    PGRP_ID = user.PGRP_ID,
                    PSNCD = user.PSN_CD,
                    USR_ID=user.USR_ID,
                    ACT_ID=user.ACT_ID,
                    MOB_NO=user.MOB_NO,
                    ACT_TYP=user.ACT_TYP,
                }), guid)),
                new Claim(ClaimTypes.Name, user.FLL_NM),
                new Claim(JwtRegisteredClaimNames.Jti, guid),
            };

            DateTime now = DateTime.Now;
            string Issuer = _config["TokenSettings:Issuer"]
                , Audience = _config["TokenSettings:Audience"]
                , Key = _config["TokenSettings:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                notBefore: now,
                expires: now.Add(TimeSpan.FromSeconds(30)),
                claims: claims,
                signingCredentials: signInCred
            );
            return new { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpirationDate = token.ValidTo, };
        }
    }
}
