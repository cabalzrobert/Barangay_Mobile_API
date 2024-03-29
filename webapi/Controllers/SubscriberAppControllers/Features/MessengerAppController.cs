using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Aggregates.FeatureAggregate;
using Comm.Commons.Advance;
using Comm.Commons.Extensions;
using webapi.App.RequestModel.Feature;
using webapi.App.Features.UserFeature;
using Newtonsoft.Json;

namespace webapi.Controllers.SubscriberAppControllers.Features
{
    [Route("app/chat")]
    [ApiController]
    [ServiceFilter(typeof(SubscriberAuthenticationAttribute))]
    public class MessengerAppController : ControllerBase
    {
        private readonly ISubscriber _identity;
        private readonly IMessengerAppRepository _appRepo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } }
        public MessengerAppController(ISubscriber identity, IMessengerAppRepository appRepo){
            _identity = identity; 
            _appRepo = appRepo;
        } 

        [HttpPost]
        [Route("public")]
        public async Task<IActionResult> Task0a(){
            var repoResult = await _appRepo.RequestPublicChatAsync();
            if(repoResult.result == Results.Success)
                return Ok(repoResult.item);
            return NotFound();
        }

        [HttpPost]
        [Route("r/{RequestID}")]
        public async Task<IActionResult> Task0b(String RequestID){
            if(RequestID.IsEmpty()) return NotFound();
            var repoResult = await _appRepo.RequestPersonalChatAsync(RequestID);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.item);
            return NotFound();
        }
        [HttpPost]
        [Route("per/{RequestID}")]
        public async Task<IActionResult> Task0b1(String RequestID)
        {
            if (RequestID.IsEmpty()) return NotFound();
            var repoResult = await _appRepo.RequestPersonalChatAsync1(RequestID);
            if (repoResult.result == Results.Success)
                return Ok(repoResult.item);
            return NotFound();
        }
        [HttpPost]
        [Route("t/{ChatKey}/{StartWith}")]
        public async Task<IActionResult> Task0c(String ChatKey, String StartWith){
            if(ChatKey.IsEmpty()) return NotFound();
            int startWith = (int)StartWith.ToDecimalDouble();
            if(startWith<0) return NotFound();
            var repoResult = await _appRepo.GetPreviousChatAsync(ChatKey, startWith);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.item);
            return NotFound();
        }
        [HttpPost]
        [Route("t/{ChatKey}")]
        public async Task<IActionResult> Task0d(String ChatKey, MessengerAppRequest request){
            //if(!MessengerAppRequest.validity(account, ChatKey, request)) return NotFound();
            var valResult = await validity(account, ChatKey, request);
            if(valResult.result == Results.Failed)
                return Ok(new { Status = "error", Message = valResult.message });
            if(valResult.result != Results.Success)
                return NotFound();

            var repoResult = await _appRepo.SendMessageAsync(ChatKey, request);
            if(repoResult.result == Results.Success)
                return Ok(repoResult.item);
            return NotFound();
        }
       
        
        [HttpPost]
        [Route("incomingcall/{ChatKey}")]
        public async Task<IActionResult> incomingcall(string ChatKey)
        {
            //await notifyIncomingCall(ChatKey, account.USR_ID);
            var result = await _appRepo.IncomingCallAsync(ChatKey);
            if (result.result == Results.Success)
                return Ok(new { status = "ok" });
            return NotFound();
        }
        
        [HttpPost]
        [Route("s/{LastChatTimestamp}")]
        public async Task<IActionResult> Task0e(String LastChatTimestamp){
            if(LastChatTimestamp.IsEmpty()) return NotFound();
            DateTime dtLastChatTimestamp = DateTime.Now.AddDays(7);
            if(!LastChatTimestamp.Equals("0"))
                try{ dtLastChatTimestamp = DateTime.Parse(LastChatTimestamp); }catch{}
            var repoResult = await _appRepo.GetPreviousChatsAsync(dtLastChatTimestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        }
        [HttpPost]
        [Route("s")]
        public async Task<IActionResult> Task0f(){
            var repoResult = await _appRepo.GetRecentChatsAsync();
            if(repoResult.result == Results.Success)
                return Ok(repoResult.items);
            return NotFound();
        }


        [HttpPost]
        [Route("isread")]
        public async Task<IActionResult> Task0g([FromBody] MessengerAppRequest req)
        {
            var result = await _appRepo.ChatMessageReadAsync(req);
            if (result.result == Results.Success)
                return Ok(new { Status = "ok", UnreadMessage = result.UnReadMessage, Message = result.message });
            if (result.result == Results.Failed)
                return Ok(new { Status = "error", UnreadMessage = result.UnReadMessage, Message = result.message });
            return NotFound();
        }
        [HttpPost]
        [Route("message/isread")]
        public async Task<IActionResult> Task0h([FromBody] MessengerAppRequest req)
        {
            var repoResult = await _appRepo.ChatMessageIsReadAsync(req);
            if (repoResult.result == Results.Success)
                return Ok(repoResult.item);
            return NotFound();
        }


        public static async Task<(Results result, string message)> validity(STLAccount account, String ChatKey, MessengerAppRequest request){
            if(request == null || ChatKey.IsEmpty()) return (Results.Null, null);
            String type = request.Type.Str();
            request.IsImage = (type == "IMG"); 
            request.IsFile = (type == "FILE");
            request.IsMessage = (type.IsEmpty() || !(request.IsImage||request.IsFile));
            
            String Message = "";
            if(request.IsMessage) Message = request.Message;
            else if(request.IsImage) Message = "{0} sent a photo.";
            else if(request.IsFile) Message = "{0} sent a file.";
            //
            request.MediaUrl = "";
            if(request.IsImage){
                var valResult = await validity(request);
                if(valResult.result != Results.Success)
                    return valResult;
            }
            request.Message = Message;
            return (Results.Success, null);
        }

        private static async Task<(Results result, string message)> validity(MessengerAppRequest request){
            if(request==null)
                return (Results.Null, null);

            if(request.Message.IsEmpty())
                return (Results.Failed, "Please select an image.");

            byte[] bytes = Convert.FromBase64String(request.Message.Str());
            if(bytes.Length == 0)
                return (Results.Failed, "Make sure selected image is valid.");
            
            var res = await ImgService.SendAsync(bytes);
            bytes.Clear();
            if(res==null)
                return (Results.Failed, "Make sure selected image is valid.");

            var json = JsonConvert.DeserializeObject<Dictionary<string,object>>(res);
            if(json["status"].Str()!="error"){
                request.MediaUrl = json["url"].Str();
                return (Results.Success, null);
            }
            return (Results.Failed, "Make sure selected image is valid.");
        }
    }
}