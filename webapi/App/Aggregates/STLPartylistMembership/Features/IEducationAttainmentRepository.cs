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
    [Service.ITransient(typeof(EducationAttainmentRepository))]
    public interface IEducationAttainmentRepository
    {
        Task<(Results result, object educattainment)> LoadEducationalAttainment(EducAttainment req);
        Task<(Results result, String message)> EducationalAttainmentAsync(EducAttainment req);
    }
    public class EducationAttainmentRepository:IEducationAttainmentRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public EducationAttainmentRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object educattainment)> LoadEducationalAttainment(EducAttainment req)
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

        public async Task<(Results result, string message)> EducationalAttainmentAsync(EducAttainment req)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDBEDUCAT0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid", account.PGRP_ID },
                {"parmuserid", account.USR_ID},
                {"parmseqnumber", req.SEQ_NO},
                {"parmeduclevel", req.EducLevel},
                {"parmschoolname", req.School},
                {"parmschooladdress", req.SchoolAddress},
                {"parmschoolyear", req.SchoolYear},
                {"parmcourse", req.Course}
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
