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

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(ConnectionRequestRepository))]
    public interface IConnectionRequestRepository
    {
        Task<(Results result, object list)> GetConnectionRequestList(int segment);
        Task<(Results result, string message, string reqid)> UpdateConnectionRequest(ConnectionRequest req);
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
