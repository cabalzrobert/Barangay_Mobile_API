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

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(DonationRepository))]
    public interface IDonationsRepository
    {
        Task<(Results result, String message)> ClaimDonation(FilterRequest request);
        Task<(Results result, object donation)> LoadDonations(FilterRequest request);
        Task<(Results result, object donation)> LoadClaimDonations(FilterRequest request);
    }
    public class DonationRepository : IDonationsRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public DonationRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, String message)> ClaimDonation(FilterRequest request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_DAABDBCBA0D", new Dictionary<string, object>()
            {
                {"parmplid", request.PL_ID },
                {"parmpgrpid", request.PGRP_ID },
                {"parmoptrid", account.USR_ID },
                {"parmusrmobno", request.MOB_NO },
                {"parmdonoid", request.DONO_ID },
                {"parmotp", request.OTP }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully Claim");
                else if (ResultCode == "2")
                    return (Results.Failed, "Code already Expired");
                else if (ResultCode == "0")
                    return (Results.Failed, "Something wrong in your data, Please try again");
            }
            return (Results.Null, null);
        }
        public async Task<(Results result, object donation)> LoadDonations(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_DAABEABDB0A", new Dictionary<string, object>() 
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid", request.Userid},
                {"parmsrch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetDonationList(result.Read<dynamic>()));
            return (Results.Null, null);
        }

        public async Task<(Results result, object donation)> LoadClaimDonations(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_DAABEABDB0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid", request.Userid},
                {"parmsrch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetDonationList(result.Read<dynamic>()));
            return (Results.Null, null);
        }
    }
}
