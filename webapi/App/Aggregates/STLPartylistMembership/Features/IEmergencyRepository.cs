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
using webapi.App.RequestModel.Feature;
using webapi.App.RequestModel.Common;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(EmergencyRepository))]
    public interface IEmergencyRepository
    {
        Task<(Results result, object list)> LoadEmergencyType(EmergencyRequest request);
        Task<(Results result, string message, object obj)> SendEmergencyAlert(EmergencyRequest request);
        Task<(Results result, object list)> LoadEmergencyAlert(FilterRequest req);

    }
    public class EmergencyRepository : IEmergencyRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public EmergencyRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object list)> LoadEmergencyType(EmergencyRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSSMEMGY0A00",new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID },
                {"parmpgrpid", account.PGRP_ID }
            });
            if (result != null)
            {
                return (Results.Success, STLSubscriberDto.GetEmergency_List(result.Read<dynamic>()));
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, object obj)> SendEmergencyAlert(EmergencyRequest request)
        {
            var results = _repo.DSpQuery<dynamic>($"dbo.spfn_BIMSMEMGY0B", new Dictionary<string, object>()
                {
                    {"parmplid", account.PL_ID},
                    {"parmpgrpid", account.PGRP_ID},
                    {"parmemgytypid", request.EmergencyId},
                    {"parmusrid", account.USR_ID},
                    {"parmsndrno", request.SenderMobileno},
                    {"parmmsgtxt", request.EmergencyMessage},
                    {"parmlat", request.latitude},
                    {"parmlong", request.longitude},
                    {"parmrptmsg", request.ReportMessage},
                    {"parmincdtimg", request.IncidentImage}
                }).FirstOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    await PostEmergencyAlert(results);
                    return (Results.Success, "Successfully Send", results);
                }
                    
                else if (ResultCode == "0")
                    return (Results.Failed, "Already exist", null);
                else if (ResultCode == "2")
                    return (Results.Null, null, null);
            }
            //return (Results.Null, null, null, null);
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object list)> LoadEmergencyAlert(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSEMGY0B04", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid",account.USR_ID },
                {"parmemgytypid",req.EmergencyType },
                {"parmrownum",req.num_row },
                {"parmsrch",req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllEmergencyAlertList(result.Read<dynamic>(), req.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<bool> PostEmergencyAlert(IDictionary<string, object> data)
        {
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/emergencyalert",
                new { type = "emergencyalert-notification", content = SubscriberDto.EmergencyAlertNotification(data) });
            return true;
        }
    }
}
