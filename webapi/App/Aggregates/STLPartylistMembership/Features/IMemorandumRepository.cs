using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.RequestModel.Common;
using Comm.Commons.Extensions;
using webapi.App.Aggregates.Common.Dto;
using webapi.Commons.AutoRegister;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(MemorandumRepository))]
    public interface IMemorandumRepository
    {
        Task<(Results result, object memo)> LoadMemorandum(FilterRequest request);
    }
    public class MemorandumRepository:IMemorandumRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public MemorandumRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object memo)> LoadMemorandum(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_MEMO0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllMemoList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }
    }
}
