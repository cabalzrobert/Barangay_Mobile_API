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
    [Service.ITransient(typeof(AccountVerificationRepository))]
    public interface IAccountVerificationRepository
    {
        Task<(Results result, string message)> SendAccountVerification(AccountVerification request);
    }
    public class AccountVerificationRepository : IAccountVerificationRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public AccountVerificationRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> SendAccountVerification(AccountVerification request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_ADDVRFYACNT", new Dictionary<string, object>()
            {
                {"parmoldusrid", account.USR_ID },
                {"parmregionid", request.Region },
                {"parmprovid", request.Province },
                {"parmmunid", request.City },
                {"parmbrgyid", request.Barangay },
                {"parmsitid", request.Sitio },
                {"parmimgselfie", request.SelfieUrl },
                {"parmimgdoc", request.DocUrl }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully send.");
                return (Results.Null, "System Error!");
            }
            return (Results.Null, null);
        }

    }
}
