using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Feature;
using webapi.Services.Dependency;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.Features.UserFeature;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.RequestModel.Common;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(BlockedUserAccountRepository))]
    public interface IBlockedUserAccountRepository
    {
        Task<(Results result, String message)> BlockedUserAccountAsync(BlockedUserAccount req);
        Task<(Results result, String message)> UnBlockedUserAccountAsync(BlockedUserAccount req);
        Task<(Results result, object blockedaccount)> LoadBlockedUserAccount(FilterRequest req);
    }
    public class BlockedUserAccountRepository:IBlockedUserAccountRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BlockedUserAccountRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> BlockedUserAccountAsync(BlockedUserAccount req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_0BD0A", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmchatid", req.ChatID },
                {"parmrequestid", req.RequestID },
                {"parmchatkey", req.ChatKey },
                {"parmuserid", account.USR_ID }
            }).ReadSingleOrDefault();
            if(results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "You have successfully blocked");
                else
                    return (Results.Failed, "Check your Data, Please try again!");
            }
            return (Results.Null, null);
        }
        public async Task<(Results result, string message)> UnBlockedUserAccountAsync(BlockedUserAccount req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_0BD0C", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmchatid", req.ChatID },
                {"parmrequestid", req.RequestID },
                {"parmchatkey", req.ChatKey },
                {"parmuserid", account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "You have successfully unblocked");
                else
                    return (Results.Failed, "Check your Data, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object blockedaccount)> LoadBlockedUserAccount(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_0BD0B", new Dictionary<string, object>()
                {
                    {"parmusrid", account.USR_ID },
                    {"parmrownum",(req.num_row == null ? "0" : req.num_row)},
                    {"parmsearch", req.Search }
                });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllBlockedAccountList(result.Read<dynamic>(), account.USR_ID, 30));

            return (Results.Null, null);
        }
    }
}
