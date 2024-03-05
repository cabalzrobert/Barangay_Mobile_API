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
using webapi.App.RequestModel.Feature;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(ProfileRepository))]
    public interface IProfileRepository
    {
        Task<(Results result, object emphistory)> LoadEmploymentHistory();
        Task<(Results result, object educattainment)> LoadEducationalAttainment();
        Task<(Results result, object govvalid)> LoadGovermentValidIDListHistory();
        Task<(Results result, object orgz)> LoadOrganization();
        Task<(Results result, object searched)> SearchedResidents(Resident req);
        Task<(Results result, object fams)> GetFamilies();
        Task<(Results result, string message, string famid)> AddFamily(Family req);
        Task<(Results result, string message, string famid)> EditDeleteFamily(Family req);
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

        public async Task<(Results result, object searched)> SearchedResidents(Resident req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BIMSSSRCHRSDNT", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid",account.USR_ID },
                {"parmsrchnm",req.Search }
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetSearchedResidentsList(results.Read<dynamic>(), account.USR_ID));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object fams)> GetFamilies()
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BIMSSGETFMLS", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid",account.USR_ID }
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetFamilyList(results.Read<dynamic>(), account.USR_ID));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string famid)> AddFamily(Family req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BIMSSADDFMLS", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmusrid", account.USR_ID},
                    //{"parmfamid", req.FamilyId},
                    {"parmmbrid", req.UserId},
                    {"parmrltsp", req.Relationship},
                    //{"parmmbrrltsp", req.MemberRelationship}
                }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "You successfully added a family member", row["FAM_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "The person you're going to add is already exist in your family", null);
            }
            //return (Results.Null, null, null, null);
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message, string famid)> EditDeleteFamily(Family req)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BIMSSUPDRMVFMLS", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmusrid", account.USR_ID},
                    {"parmfamid", req.FamilyId},
                    {"parmmbrid", req.UserId},
                    {"parmrltsp", req.Relationship},
                    {"parmsrmv", req.IsRemoved}
                    //{"parmmbrrltsp", req.MemberRelationship}
                }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "The family member has been updated", row["FAM_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Failed to update the family member", null);
            }
            //return (Results.Null, null, null, null);
            return (Results.Null, null, null);
        }

    }
}
