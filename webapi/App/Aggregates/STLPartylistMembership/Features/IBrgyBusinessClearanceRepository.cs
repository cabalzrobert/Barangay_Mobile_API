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
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(BrgyBusinessClrearanceRepository))]
    public interface IBrgyBusinessClearanceRepository
    {
        Task<(Results result, string message)> RequstBrgyBusinessClearanceAsync(BrgyBusinessClearance req, bool isUpdate = false);
        Task<(Results result, object brybizclrid)> Load_BrgyClearance(BrgyBusinessClearance req);
        Task<(Results result, object biz)> LaodBusiness(FilterRequest request);
    }
    public class BrgyBusinessClrearanceRepository:IBrgyBusinessClearanceRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BrgyBusinessClrearanceRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> RequstBrgyBusinessClearanceAsync(BrgyBusinessClearance req, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYBIZCLR0A1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgybizclrid", req.BusinessClearanceID },
                {"parmcontrolno", req.ControlNo },
                {"parmbusinessid", req.BusinessID },
                {"parmoptrid",account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (req.BusinessClearanceID == "" || req.BusinessClearanceID == null)
                    {
                        req.BusinessClearanceID = row["BIZCLR_ID"].Str();
                        req.ControlNo = row["CNTRL_NO"].Str();
                        req.ApplicationDate = (row["ApplicationDate"].Str() == "") ? "" : Convert.ToDateTime(row["ApplicationDate"].Str()).ToString("MMM dd, yyyy");
                        req.Status = "0";
                        req.StatusRequest = "0";
                    }
                    PostRequestDocument(results);
                    return (Results.Success, "Clearance succesfully save!");
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<bool> PostRequestDocument(IDictionary<string, object> data)
        {
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/requestdocument",
                new { type = "requestdocument-notification", content = SubscriberDto.RequestDocumentNotification(data) });
            return true;
        }

        public async Task<(Results result, object brybizclrid)> Load_BrgyClearance(BrgyBusinessClearance req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYBIZCLR0C1", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmownerid",account.USR_ID },
                {"parmrequeststatus", req.Status }
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetBrygBusinessClearanceList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object biz)> LaodBusiness(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIZOC1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmownerid", account.USR_ID },
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllRegisterBusinessList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }
    }
}
