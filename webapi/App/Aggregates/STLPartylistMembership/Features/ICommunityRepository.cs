using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.Commons.AutoRegister;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(CommunityRepository))]
    public interface ICommunityRepository
    {
        Task<(Results result, object comm)> LoadCommunityListAsync(FilterRequest req);
        Task<(Results result, String message)> SendRequestJoinCommunityAsync(Community req);
        Task<(Results result, String CountCommunity)> GetCountCommunityAsync();
        Task<(Results result, string Message)> LeaveCommunityAsync(PostCommentCommunity req);
    }
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public CommunityRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object comm)> LoadCommunityListAsync(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRAC000B1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
                {"parmrownum",req.num_row },
                {"parmsearch",req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllCommunityList(result.Read<dynamic>(), req.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> SendRequestJoinCommunityAsync(Community req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRAC000A3", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmcommunityjson",req.CommunityJson },
                {"parmuserid",account.USR_ID },
            }).ReadFirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Join community successful. Start exploring!");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Please check your data entry. Please try again");
                return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string CountCommunity)> GetCountCommunityAsync()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRAC000J1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
            }).ReadFirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode != "0")
                {
                    return (Results.Success, ResultCode);
                }
                else
                {
                    return (Results.Failed, ResultCode);
                }
            }
            return (Results.Null, "0");
        }

        public async Task<(Results result, string Message)> LeaveCommunityAsync(PostCommentCommunity req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRAC000A4", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmcommid",req.CommunityID },
                {"parmuserid",account.USR_ID },
            }).ReadFirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "You leave this community");
                }
                else
                {
                    return (Results.Failed, "Please check your data entry. Please try again");
                }
            }
            return (Results.Null, null);
        }
    }
}
