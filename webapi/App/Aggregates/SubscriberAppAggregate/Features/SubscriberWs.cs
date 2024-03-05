using System;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
//
using webapi.App.Features.WebSocketFeature;
using webapi.App.Model.User;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.SubscriberAppAggregate.Features
{
    [Service.ITransient(typeof(SubscriberWs))] 
    public interface ISubscriberWs
    {
        Task Echo(ControllerBase controller, STLAccount account, WebSocket ws);
    }

    public class SubscriberWs: ISubscriberWs
    {   
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly StompPusher _pusher;
        //
        public SubscriberWs(StompPusher pusher, IServiceScopeFactory serviceScopeFactory){
            _serviceScopeFactory = serviceScopeFactory;
            _pusher = pusher;
        }
        public async Task Echo(ControllerBase controller, STLAccount account, WebSocket ws){
            using(var handler = new WebSocketHandler(controller, ws)){
                try{
                    var stomp = new SubscriberSc(_pusher, account, handler, _serviceScopeFactory);
                    handler.onEmit(async(bytes)=>{
                        stomp.onMessageReceived(stomp.parse(bytes));
                        await Task.Delay(1);
                    });
                    handler.onClose(async(handler)=>{
                        stomp.Detach();
                        await Task.Delay(1);
                    });
                    await handler.start();
                }catch(Exception e){
                    string str = e.StackTrace;
                }
            }
        }
    }
    public class SubscriberSc: StompClient
    {
        private IServiceScopeFactory scopeFactory;
        private WebSocketHandler handler;
        private STLAccount account;
        private StompPusher.StackableSubscribe stack;
        public SubscriberSc(StompPusher pusher, STLAccount account, WebSocketHandler handler, IServiceScopeFactory scopeFactory){
            setWebSocketHandler(this.handler = handler);
            if(account.IsSessionExpired) return;
            this.stack = pusher.create();
            this.account = account;
            this.scopeFactory = scopeFactory;
            //
            this.Subscribes();
        }
        private bool isConnected = false;
        public void onMessageReceived(StompMessage message){
            if(message==null) return;
            string command = message.getStompCommand().Str();
            if(command.Equals(StompCommand.CONNECT)){
                if(isConnected) return;
                isConnected = true;
                stomp(new StompMessage(StompCommand.CONNECTED, message.getStompHeaders(), null)
                    .addHeader("session", ((int)DateTime.Now.ToTimeMillisecond()).ToString("X")));
            }
        }

        private void Subscribes(){

            stack.subscribe("/test", this.msgTest);
            
            stack.subscribe($"/{account.PL_ID}/notify", this.receivedCompanyNotication);
            //stack.subscribe($"/{account.PL_ID}/{account.PGRP_ID}/chat", this.receivedBranchPublicChat);
            stack.subscribe($"/{account.PL_ID}/chat", this.receivedBranchPublicChat);
            stack.subscribe($"/{account.PL_ID}/commentpostcommunity", this.receivedCommentPostCommunity);
            stack.subscribe($"/{account.PL_ID}/postcommunity/reaction", this.receivedReactionPostCommunity);
            stack.subscribe($"/{account.PL_ID}/postcommunity/comment/reaction", this.receivedReactionCommentPostCommunity);
            stack.subscribe($"/{account.PL_ID}/{account.PGRP_ID}/notify", this.receivedBranchNotication);
            //stack.subscribe($"/{account.PL_ID}/{account.PGRP_ID}/{account.USR_ID}/chatmessageread", this.ChatMessageIsReadNotication);

            stack.subscribe($"/{account.PL_ID}/{account.USR_ID}/chatmessageread", this.ChatMessageIsReadNotication);
            //stack.subscribe($"/{account.CompanyID}/{account.BranchID}/arena", this.receivedBranchArena);
            //stack.subscribe($"/{account.PL_ID}/{account.PGRP_ID}/{account.USR_ID}/chat", this.receivedSubscriberChat);
            /*Send Message Notification*/
            stack.subscribe($"/{account.PL_ID}/{account.USR_ID}/receivedchat", this.receivedSubscriberReceivedChat);
            stack.subscribe($"/{account.PL_ID}/{account.PGRP_ID}/{account.USR_ID}/notify", this.receivedSubscriberNotification);
            //stack.subscribe($"/{account.PL_ID}/{account.USR_ID}/strangerrequest/notify", this.receivedSubscriberStrangerRequest);
            stack.subscribe($"/{account.PL_ID}/strangerrequest/notify", this.receivedSubscriberStrangerRequest);
            stack.subscribe($"/{account.PL_ID}/{account.PGRP_ID}/{account.USR_ID}/balance", this.receivedSubscriberBalance);
            stack.subscribe($"/{account.PL_ID}/{account.PGRP_ID}/{account.USR_ID}/livestream", this.receivedSubscriberBalance);
            stack.subscribe($"/{account.PL_ID}/{account.PGRP_ID}/{account.USR_ID}/virtualid", this.receivedSubscriberVirtualID);


            //if(!account.GeneralCoordinatorID.IsEmpty())
            //    stack.subscribe($"/{account.CompanyID}/{account.BranchID}/{account.GeneralCoordinatorID}/downline", this.receivedSubscriberNotification);
            //if(!account.CoordinatorID.IsEmpty() && !account.CoordinatorID.Equals(account.SubscriberID))
            //    stack.subscribe($"/{account.CompanyID}/{account.BranchID}/{account.CoordinatorID}/downline", this.receivedSubscriberNotification);
            //stack.subscribe($"/{account.CompanyID}/{account.BranchID}/{account.GeneralCoordinatorID}/upline", this.receivedSubscriberChat);
            //stack.subscribe($"postbet/notify", (Ultralight.StompMessage message)=>{});
        }
        public void Detach(){
            this.unsubscribes();
            if(stack!=null){
                using(stack){
                    stack.unsubscribes();
                }
            }
        }
        //
        private void msgTest(Ultralight.StompMessage message){
            stomp("/test", message.Body);
        }
        
        private void receivedCompanyNotication(Ultralight.StompMessage message){
            stomp("/company", message.Body);
        }
        private void receivedBranchNotication(Ultralight.StompMessage message){
            stomp("/branch", message.Body);
        }
        private void ChatMessageIsReadNotication(Ultralight.StompMessage message)
        {
            stomp("/chatmessageread", message.Body);
        }
        private void receivedBranchArena(Ultralight.StompMessage message){
            stomp("/arena", message.Body);
        }
        private void receivedBranchPublicChat(Ultralight.StompMessage message){
            stomp("/chat/pub", message.Body);
        }
        private void receivedCommentPostCommunity(Ultralight.StompMessage message)
        {
            stomp("/commentpostcommunity", message.Body);
        }
        private void receivedReactionCommentPostCommunity(Ultralight.StompMessage message)
        {
            stomp("/postcommunity/comment/reaction", message.Body);
        }
        private void receivedReactionPostCommunity(Ultralight.StompMessage message)
        {
            stomp("/postcommunity/reaction", message.Body);
        }
        //
        private void receivedSubscriberChat(Ultralight.StompMessage message){
            stomp("/chat", message.Body);
        }
        private void receivedSubscriberReceivedChat(Ultralight.StompMessage message)
        {
            stomp("/receivedchat", message.Body);
        }
        private void receivedSubscriberVirtualID(Ultralight.StompMessage message)
        {
            stomp("/virtualid", message.Body);
        }
        private void receivedSubscriberNotification(Ultralight.StompMessage message){
            stomp("/notify", message.Body);
        }
        private void receivedSubscriberStrangerRequest(Ultralight.StompMessage message)
        {
            stomp("/strangerrequest/notify", message.Body);
        }
        private void receivedSubscriberBalance(Ultralight.StompMessage message){
            stomp("/balance", message.Body);
        }
        private void receivedSubscriberLiveStreaming(Ultralight.StompMessage message)
        {
            stomp("/lottery/profile", message.Body);
        }
    }

}