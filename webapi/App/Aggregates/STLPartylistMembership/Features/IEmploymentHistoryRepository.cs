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
using Org.BouncyCastle.Ocsp;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(EmploymentHistoryRespository))]
    public interface IEmploymentHistoryRepository
    {
        Task<(Results result, object emphistory)> LoadEmploymentHistory();
        Task<(Results result, String message)> EmploymentHistoryAsync(Employment_History req);
    }
    public class EmploymentHistoryRespository:IEmploymentHistoryRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public EmploymentHistoryRespository(ISubscriber identity, IRepository repo)
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

        public async Task<(Results result, string message)> EmploymentHistoryAsync(Employment_History req)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBEMPHST0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmuserid", account.USR_ID},
                {"parmseqnumber", req.SEQ_NO},
                {"parmcompany", req.Company},
                {"parmaddress", req.CompanyAddress},
                {"parmrenderedfrom", string.IsNullOrEmpty(req.RenderedFrom)?null:Convert.ToDateTime(req.RenderedFrom).ToString("MMM dd, yyyy")},
                {"parmrenderedto", string.IsNullOrEmpty(req.RenderedTo)?null:Convert.ToDateTime(req.RenderedTo).ToString("MMM dd, yyyy")}
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    req.SEQ_NO = Convert.ToInt32(row["SEQ_NO"].Str());
                    req.RenderedFrom = Convert.ToDateTime(req.RenderedFrom).ToString("MMM dd, yyyy");
                    req.RenderedTo = (req.RenderedTo.IsEmpty()) ? "Present" : Convert.ToDateTime(req.RenderedTo).ToString("MMM dd, yyyy");
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
