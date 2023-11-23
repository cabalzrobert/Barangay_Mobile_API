using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.Model.User;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(BrgyOperatorRepository))]
    public interface IBrgyOperatorRepository
    {
        Task<(Results result, object bryoptr)> Load_BrgyOperator(FilterRequest req);
        Task<(Results result, object prev)> Load_PrevComAccount(FilterRequest req);
    }
    public class BrgyOperatorRepository: IBrgyOperatorRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo; 
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BrgyOperatorRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object bryoptr)> Load_BrgyOperator(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BDB0E", new Dictionary<string, object>()
                {
                    {"parmplid",account.PL_ID },
                    {"parmpgrpid",account.PGRP_ID },
                    {"parmoptrid", account.USR_ID },
                    {"parmsearch", req.Search }
                });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetBrgyOperatorList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);
        }

        public async Task<(Results result, object prev)> Load_PrevComAccount(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_0BA0BBBDB02", new Dictionary<string, object>()
                {
                    //{"parmplid",account.PL_ID },
                    //{"parmpgrpid",account.PGRP_ID },
                    {"parmusrid", $"{account.PL_ID}{account.USR_ID}" },
                    {"parmsearch", req.Search }
                });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllChatSenderList(result.Read<dynamic>()));

            return (Results.Null, null);
        }
    }
}
