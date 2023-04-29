using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.Model.User;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Feature;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(RequestCedulaRepository))]
    public interface IRequestCedulaRepository
    {
        Task<(Results result, String message)> Request(CedulaRequest request);
        Task<(Results result, object list)> Load(CedulaRequest request);
        Task<(Results result, string series)> GenerateSeries(CedulaRequest request);
        Task<(Results result, string message)> Cancel(CedulaRequest request);
    }
    public class RequestCedulaRepository : IRequestCedulaRepository
    {
        //private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        //public STLAccount account { get { return _identity.AccountIdentity(); } }
        public RequestCedulaRepository(/*ISubscriber identity,*/ IRepository repo)
        {
            //_identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> Request(CedulaRequest request)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYRQCTCBDB", new Dictionary<string, object>()
                {
                    {"parmplid", request.PL_ID},
                    {"parmpgrpid", request.PGRP_ID},
                    {"parmmobctcid", request.RequestId},
                    {"parmreqby", request.RequestBy},
                    {"parmgeb", request.GrossBusinessIncome},
                    {"parmgep", request.Salary},
                    {"parmirp", request.RealPropertyIncome},
                    {"parmreqdt", DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt")}
                }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Submission Success");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already exist");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            //return (Results.Null, null, null, null);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> Cancel(CedulaRequest request)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BRGYRQCTC01", new Dictionary<string, object>()
                {
                    {"parmplid", request.PL_ID},
                    {"parmpgrpid", request.PGRP_ID},
                    {"parmctcno", request.RequestId},
                    {"parmrfc", request.CancelledReason},
                    {"parmoprtr", request.RequestBy},
                    {"parmdtcncl", DateTime.Now.ToString("MMMM dd, yyyy hh:mm:ss tt")}
                }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successful Cancellation");
                else if (ResultCode == "0")
                    return (Results.Failed, "Already Cancelled");
                else if (ResultCode == "2")
                    return (Results.Null, null);
            }
            //return (Results.Null, null, null, null);
            return (Results.Null, null);
        }

        public async Task<(Results result, object list)> Load(CedulaRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGYCTCM00", new Dictionary<string, object>()
            {
                {"parmplid", request.PL_ID},
                {"parmpgrpid", request.PGRP_ID},
                {"parmusrid", request.RequestBy},
                {"parmtyp", request.Type}
            });

            if (result != null)
                return (Results.Success, STLSubscriberDto.GetRequstCedula_List(result.Read<dynamic>()));
            return (Results.Null, null);
        }

        public async Task<(Results result, string series)> GenerateSeries(CedulaRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_GETCTCCTR", new Dictionary<string, object>()
            {
                {"parmplid", request.PL_ID},
                {"parmpgrpid", request.PGRP_ID}
            });

            if (result != null)
                return (Results.Success, STLSubscriberDto.GetSeries(result.Read<dynamic>()));
            return (Results.Null, null);
        }
    }
}
