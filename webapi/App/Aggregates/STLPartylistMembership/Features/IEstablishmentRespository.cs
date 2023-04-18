using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using Comm.Commons.Extensions;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using Infrastructure.Repositories;
using webapi.App.Model.User;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.RequestModel.Common;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(EstablishmentRepository))]
    public interface IEstablishmentRespository
    {
        Task<(Results result, object est)> Load_Establishment(FilterRequest req);
    }
    public class EstablishmentRepository:IEstablishmentRespository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public EstablishmentRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object est)> Load_Establishment(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSEST0D", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID},
                {"parmrownum",req.num_row},
                {"parmsrch", req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllEstablishmentList(result.Read<dynamic>()));
            return (Results.Null, null);
        }
    }
}
