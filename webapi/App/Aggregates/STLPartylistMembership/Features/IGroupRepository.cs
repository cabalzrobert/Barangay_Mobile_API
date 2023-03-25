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
    [Service.ITransient(typeof(GroupRepository))]
    public interface IGroupRepository
    {
        Task<(Results result, object ldr)> SiteLeader(Group grp); 
    }
    public class GroupRepository : IGroupRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public GroupRepository (ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }
        public async Task<(Results result, object ldr)> SiteLeader(Group grp)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BEABDBLLL0B", new Dictionary<string, object>()
            {
                {"parmplid",grp.PL_ID },
                {"parmpgrpid",grp.PGRP_ID },
                {"parmusrid", grp.USR_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetGroupList(result.Read<dynamic>()));
            return (Results.Null, null);
        }

    }
}
