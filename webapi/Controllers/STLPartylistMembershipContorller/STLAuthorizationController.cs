using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

using Comm.Commons.Advance;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.STLPartylistMembership;
using webapi.App.Model.User;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;

namespace webapi.Controllers.STLPartylistMembership
{
    [Route("app/v1/stl")]
    [ApiController]
    public class STLAuthorizationController:ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _repo;
        public STLAuthorizationController(IConfiguration config, IAccountRepository repo)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost]
        [Route("account/changepassword")]
        public async Task<IActionResult> ForgotPasswordConfirm([FromBody] RequiredChangePassword request)
        {
            var repoResult = await _repo.RequiredChangePassword(request);
            if (repoResult.result == Results.Success)
            {
                return Ok(new { Status = "ok", Message = repoResult.message });
            }
            else if (repoResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = repoResult.message });
            return NotFound();
        }


        [HttpPost]
        [Route("account/apkupdate")]
        public async Task<IActionResult> ApkUpdate([FromBody] STLSignInRequest request)
        {
            if (request == null) return NotFound();
            if (!(request.DeviceID.Str().Equals("web") || request.DeviceName.Str().Equals("web")))
            {
                var chkResult = await _repo.ApkUpdateCheckerAsync(request);
                if (chkResult.result == SignInResults.ApkUpdate)
                {
                    return Ok(new { Status = "ok", Mode = "apkupdate", Message = chkResult.message, ApkVersion = chkResult.apkVersion, ApkUrl = chkResult.apkUrl, });
                }
                else if (chkResult.result != SignInResults.Success)
                    return NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        [Route("stlsignin")]
        public async Task<IActionResult> Task0a([FromBody] STLSignInRequest request)
        {
            if (request == null) return NotFound();

            var res = await _repo.GetSubscriberID(request);
            if(res.result !=Results.Success)
                return Ok(new { Status = "error", Message = res.message });

            if (!(request.DeviceID.Str().Equals("web") || request.DeviceName.Str().Equals("web")))
            {
                request.ApkVersion = request.ApkVersion.Replace(" ", "_");
                var chkResult = await _repo.ApkUpdateCheckerAsync(request);
                if (chkResult.result == SignInResults.ApkUpdate)
                {
                    return Ok(new { Status = "ok", Mode = "apkupdate", Message = chkResult.message, ApkVersion = chkResult.apkVersion, ApkUrl = chkResult.apkUrl, });
                }
                else if (chkResult.result != SignInResults.Success)
                    return NotFound();
            }


            var result = await _repo.STLSignInAsync(request);
            if (result.result == SignInResults.Success)
            {
                var token = CreateToken(result.account);
                var data = await _repo.MemberGroup(result.account);
                return Ok(new { Status = "ok", Account=result.account, Auth = token, Company = data.PartyList, Group=data.Group });
            }
            else if (result.result == SignInResults.ChangePassword)
                return Ok(new { Status = "ok", Mode = "change-password", Message = result.message, PLID = request.plid, PGRPID = request.groupid, PSNCD = request.psncd });
            else if (result.result == SignInResults.Failed)
                return Ok(new { Status = "error", Message = result.message });
            return NotFound();
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
                    SUB_TYP=user.SUB_TYP,
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
