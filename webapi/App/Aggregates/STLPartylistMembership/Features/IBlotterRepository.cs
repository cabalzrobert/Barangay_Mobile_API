using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.SubscriberApp;
using webapi.App.RequestModel.SubscriberApp.Common;
using webapi.App.Model.User;
using webapi.App.Globalize.Company;

using Newtonsoft.Json;
using System.IO;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

using Comm.Commons.Advance;
using webapi.App.RequestModel.Feature;
using webapi.Services.Firebase;
using webapi.Services.Dependency;
using System.Net;

using System.Net.Mail;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(BlotterRepository))]
    public interface IBlotterRepository
    {
        Task<(Results result, String message)> ComplaintAsync(ComplaintBlotter req, bool isUpdate=false);
        Task<(Results result, object blotter)> LoadComplaint(ComplaintBlotter req);
        Task<(Results result, object blotter)> LoadRespondent(ComplaintBlotter req);
        Task<(Results result, object blotter)> LoadComplaintAttachment(ComplaintBlotter req);
    }
    public class BlotterRepository:IBlotterRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        private readonly IFileData _fd;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BlotterRepository(ISubscriber identity, IRepository repo, IFileData fd)
        {
            _identity = identity;
            _repo = repo;
            _fd = fd;
        }

        public async Task<(Results result, string message)> ComplaintAsync(ComplaintBlotter req, bool isUpdate=false)
        {
            string strincidentdate = Convert.ToDateTime(req.IncidentDate).ToString("yyyyMMdd");
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BRGYBLOTTER0A1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgycaseno",isUpdate ? req.CaseNo : "" },
                {"parmcomplainant", req.ComplainantID },
                {"parmrespondent", req.Respondent },
                {"parmwtns", req.Witness },
                {"parmcomplainttype", req.ComplaintType },
                {"parmincidentplace", req.IncidentPlace },
                {"parmincidentdate", Convert.ToDateTime(req.IncidentDate).ToString("yyyyMMdd") },
                {"parmincidenttime", req.IncidentTime },
                {"parmissue", req.Issue },
                {"parmblotterdetails", req.Statement },
                {"parmxattchmnt", req.iAttachments }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    req.IncidentTime = $"{req.IncidentDate} {req.IncidentTime}";
                    if (!isUpdate)
                    {
                        req.CaseNo = row["BRGY_CASE_NO"].Str();
                    }
                    PostComplaint(results);
                    return (Results.Success, "Complaint Successfully Save");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Check your Data, Please try again!");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!");
            }
            return (Results.Success, null);
        }

        public async Task<bool> PostComplaint(IDictionary<string, object> data)
        {
            await Pusher.PushAsync($"/{account.PL_ID}/{account.PGRP_ID}/complaint",
                new { type = "complaint-notification", content = SubscriberDto.ComplaintNotification(data) });
            return true;
        }

        public async Task<(Results result, object blotter)> LoadComplaint(ComplaintBlotter req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGYBLOTTER0B01", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid", req.ComplainantID},
                {"parmrownum", req.num_row},
                {"parmsrch", req.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllComplaintList(result.Read<dynamic>(), req.ComplainantID, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object blotter)> LoadComplaintAttachment(ComplaintBlotter req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGYBLOTTER0C", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmbrgycaseno", req.CaseNo}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAttachementComplaintList(result.Read<dynamic>(), 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object blotter)> LoadRespondent(ComplaintBlotter req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGYBLOTTER0B02", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid", req.ComplainantID},
                {"parmrownum", req.num_row},
                {"parmsrch", req.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllComplaintList(result.Read<dynamic>(), req.ComplainantID, 100));
            return (Results.Null, null);
        }
    }
}
