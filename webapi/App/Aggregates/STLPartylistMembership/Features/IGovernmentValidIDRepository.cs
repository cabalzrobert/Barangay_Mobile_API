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
    [Service.ITransient(typeof(GovernmentValidIDRepository))]
    public interface IGovernmentValidIDRepository
    {
        Task<(Results result, object govvalid)> LoadGovermentValidIDList(Government_Valid_ID req);
        Task<(Results result, object govvalid)> LoadGovermentValidIDListHistory(Government_Valid_ID req);
        Task<(Results result, String message)> GovermentValidIDAsync(Government_Valid_ID req);
    }
    public class GovernmentValidIDRepository:IGovernmentValidIDRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public GovernmentValidIDRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object govvalid)> LoadGovermentValidIDList(Government_Valid_ID req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_GOVVALID0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrownum", req.num_row },
                {"parmsearch", req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllGovenmentIDList(result.Read<dynamic>(), account.USR_ID, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object govvalid)> LoadGovermentValidIDListHistory(Government_Valid_ID req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BDBGOVID0B", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
                {"parmrownum",(req.NextFilter == null) ? "0" : req.NextFilter},
                {"parmsearch",req.Search }
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetGovernmentIssuedList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, String message)> GovermentValidIDAsync(Government_Valid_ID req)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBGOVID0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmuserid", account.USR_ID},
                {"parmseqnumber", req.SEQ_NO},
                {"parmgovernmentvalidid", req.GovernmentID},
                {"parmidnumber", req.GovValIDNo},
                {"parmattachment", req.GovValIDURL}
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
    }
}
