using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.RequestModel.Common;
using webapi.App.RequestModel.Feature;
using webapi.App.Aggregates.FeatureAggregate;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(ConnectionRequestRepository))]
    public interface IConnectionRequestRepository
    {
        Task<(Results result, object list)> GetConnectionRequestList(int segment);
        Task<(Results result, string message, string reqid)> UpdateConnectionRequest(ConnectionRequest req);
        Task<(Results result, string message)> SendConnectionRequestAsync(ConnectionRequest req);
        Task<(Results result, object count)> UnattendedRequestAsync();
    }
    public class ConnectionRequestRepository : IConnectionRequestRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public ConnectionRequestRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object list)> GetConnectionRequestList(int segment)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_GETMOBCHTRQLST", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid",account.USR_ID },
                {"parmsgmnt",segment}
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.ConnectionRequestParser(results.Read<dynamic>(), account.USR_ID));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string reqid)> UpdateConnectionRequest(ConnectionRequest req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_UPDATEMOBCHTRQLST", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmreqid", req.ConnectionRequestId},
                    {"parmagenda", req.Agenda},
                    {"parmsacptd", req.IsAccepted?1:0},
                    {"parmsdclnd", req.IsDeclined?1:0},
                    {"parmscncld", req.IsCanceled?1:0},
                    {"parmsupd", req.IsUpdate?1:0}
                }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Success", row["REQ_BY"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Failed",null);
            }
            //return (Results.Null, null, null, null);
            return (Results.Null, null, null);
        }
        public async Task<(Results result, string message)> SendConnectionRequestAsync(ConnectionRequest req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_ADDMOBCHTRQLST", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmgrpid", account.PGRP_ID},
                    {"parmreqby", req.RequestByID},
                    {"parmreqto", req.RequestToID},
                    {"parmagenda", req.Agenda},
                }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    req.ConnectionRequestId = row["REQ_ID"].Str();
                    await Stranger_Connection_Request(row, req, account.PL_ID, req.RequestToID);
                    return (Results.Success, "Success");
                }
                else if (ResultCode == "2")
                {
                    req.ConnectionRequestId = row["REQ_ID"].Str();
                    return (Results.Success, "Success");
                }

                else if (ResultCode == "0")
                    return (Results.Failed, "Failed");
            }
            //return (Results.Null, null, null, null);
            return (Results.Null, null);
        }

        public async Task<(Results result, object count)> UnattendedRequestAsync()
        {
            var result = _repo.DSpQueryMultiple("dbo.spfn_STRNGRQSTCNT", new Dictionary<string, object>(){
                { "parmplid", account.PL_ID },
                { "parmpgrpid", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
            }).ReadSingleOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                return (Results.Success, row["UNATTENDEDRQST"].Str());
            }
            return (Results.Null, null);
        }

        public async Task<bool> Stranger_Connection_Request(IDictionary<string, object> row, object content, string plid, string userid)
        {
            //var settings = STLSubscriberDto.GetGroup(row);
            //var notifications = SubscriberDto.EventNofitication(row);

            /*
             await Pusher.PushAsync($"/{plid}/{userid}/strangerrequest/notify"
                , new { type = "stranger-request", content = content });
             */

            await Pusher.PushAsync($"/{plid}/strangerrequest/notify"
                , new { type = "stranger-request", content = content });
            return false;
        }

        //public async Task<(Results result, string message)> EditDeleteFamily(Family req)
        //{
        //    var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BIMSSUPDRMVFMLS", new Dictionary<string, object>()
        //        {
        //            {"parmplid", account.PL_ID},
        //            {"parmpgrpid", account.PGRP_ID},
        //            {"parmusrid", account.USR_ID},
        //            {"parmfamid", req.FamilyId},
        //            {"parmmbrid", req.UserId},
        //            {"parmrltsp", req.Relationship},
        //            {"parmsrmv", req.IsRemoved}
        //            //{"parmmbrrltsp", req.MemberRelationship}
        //        }).FirstOrDefault();
        //    if (results != null)
        //    {
        //        var row = ((IDictionary<string, object>)results);
        //        string ResultCode = row["RESULT"].Str();
        //        if (ResultCode == "1")
        //            return (Results.Success, "Successfully Send");
        //        else if (ResultCode == "0")
        //            return (Results.Failed, "System Error");
        //    }
        //    //return (Results.Null, null, null, null);
        //    return (Results.Null, null);
        //}

    }
}
