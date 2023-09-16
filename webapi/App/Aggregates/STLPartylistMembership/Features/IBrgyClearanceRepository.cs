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
    [Service.ITransient(typeof(BrgyClearanceRepository))]
    public interface IBrgyClearanceRepository
    {
        Task<(Results result, String message, String brgyclrid, String cntrlno)> RequestBrgyClearanceAsync(BrgyClearance req, bool isUpdate = false);
        Task<(Results result, object bryclrid)> Load_BrgyClearance(FilterRequest req);
        Task<(Results result, object certtyp)> LoadCertificateType();
        Task<(Results result, object purpose)> LoadPurpose();
    }
    public class BrgyClearanceRepository : IBrgyClearanceRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BrgyClearanceRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, string brgyclrid, string cntrlno)> RequestBrgyClearanceAsync(BrgyClearance req, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYCLR0F1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgyclrid", req.ClearanceNo },
                {"parmcontrolno", req.ControlNo },
                {"parmtypeclearanceid",req.TypeofClearance },
                {"parmpurposeid",req.PurposeID },
                {"parmuserid", account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (!isUpdate)
                    {
                        req.ClearanceNo = row["BRGYCLR_ID"].Str();
                        req.ControlNo = row["CNTRL_NO"].Str();
                        req.ApplicationDate = DateTime.Now.ToString("MMM dd, yyyy");
                        req.Requestor = req.RequestorNM;
                    }
                    PostRequestBarangayClearance(results);
                    return (Results.Success, "Clearance succesfully save!", req.ClearanceNo, req.ControlNo);
                }
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!", null, null);
            }
            return (Results.Null, null, null, null);
        }

        public async Task<bool> PostRequestBarangayClearance(IDictionary<string, object> data)
        {
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/requestdocument",
                new { type = "requestdocument-notification", content = SubscriberDto.RequestDocumentNotification(data) });
            return true;
        }

        public async Task<(Results result, object bryclrid)> Load_BrgyClearance(FilterRequest req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYCLR0G", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",req.Userid },
                {"parmrownum",req.num_row },
                {"parmrequeststatus", req.Status },
                {"parmsearch", req.Search },
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetBrygClearanceList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object certtyp)> LoadCertificateType()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_CERTTYP0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetCertificateTypeList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);
        }

        public async Task<(Results result, object purpose)> LoadPurpose()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_PURPOSE0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetPurposeList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);
        }
    }
}
