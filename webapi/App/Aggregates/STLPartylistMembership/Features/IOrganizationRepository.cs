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
    [Service.ITransient(typeof(OrganizationRepository))]
    public interface IOrganizationRepository
    {
        Task<(Results result, object orgz)> LoadOrganizationHistory(Organization req);
        Task<(Results result, object orgz)> LoadOrganization(Organization req);
        Task<(Results result, String message)> OrganizationAsync(Organization req);
    }
    public class OrganizationRepository:IOrganizationRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public OrganizationRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object orgz)> LoadOrganization(Organization req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BDBORGZ0B", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
                {"parmrownum",req.NextFilter },
                {"parmsearch",req.Search }
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetOrganizationHistoryList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> OrganizationAsync(Organization req)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBORGZ0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmuserid", account.USR_ID},
                {"parmseqnumber", req.SEQ_NO},
                {"parmorganizationid", req.OrganizationID}
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    req.SEQ_NO = Convert.ToInt32(row["SEQ_NO"].Str());
                    return (Results.Success, "Successfully save!");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Please check your data entry. Please try again");
                return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object orgz)> LoadOrganizationHistory(Organization req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BDBORGZ0D", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmrownum",(req.NextFilter == null) ? "0" : req.NextFilter},
                {"parmsearch",req.Search }
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetOrganizationList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }
    }
}
