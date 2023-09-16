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

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(ProfileRepository))]
    public interface IProfileRepository
    {
        Task<(Results result, object emphistory)> LoadEmploymentHistory();
        Task<(Results result, object educattainment)> LoadEducationalAttainment();
        Task<(Results result, object govvalid)> LoadGovermentValidIDListHistory();
        Task<(Results result, object orgz)> LoadOrganization();
    }
    public class ProfileRepository : IProfileRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public ProfileRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object emphistory)> LoadEmploymentHistory()
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BDBEMPHST0B", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetEmploymentHistoryList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object educattainment)> LoadEducationalAttainment()
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BDBEDUCAT0B", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetEducationAttainmentList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object govvalid)> LoadGovermentValidIDListHistory()
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BDBGOVID0B", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
                {"parmrownum",0}
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetGovernmentIssuedList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object orgz)> LoadOrganization()
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BDBORGZ0B", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
                {"parmrownum",0},
                {"parmsearch",null}
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetOrganizationHistoryList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

    }
}
