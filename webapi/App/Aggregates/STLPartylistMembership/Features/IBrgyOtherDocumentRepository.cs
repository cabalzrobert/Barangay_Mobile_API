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
    [Service.ITransient(typeof(BrgyOtherDocumentRepository))]
    public interface IBrgyOtherDocumentRepository
    {
        Task<(Results result, String message)> RequestBrgyOtherDocumentAsync(LegalDocument_Transaction req, bool isEdit = false);
        Task<(Results result, object lgldoctrans)> Load_OtherDocumentRequest(LegalDocument_Transaction req);
        Task<(Results result, object tpl)> Load_TemplateType();
        Task<(Results result, object tpldoc)> Load_TemplateDoc(string templatetypeid);
    }
    public class BrgyOtherDocumentRepository:IBrgyOtherDocumentRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BrgyOtherDocumentRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message)> RequestBrgyOtherDocumentAsync(LegalDocument_Transaction req, bool isEdit = false)
        {
            var results = _repo.DSpQueryMultiple($"spfn_LGLDOC0A1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmlgldocid", (!isEdit) ? "" : req.LegalDocumentID },
                {"parmcontrolno", (!isEdit) ? "" : req.ControlNo },
                {"parmtemplatetype",req.TemplateTypeID },
                {"parmtemplatedoc",req.TemplateID },
                {"parmrequestor",account.USR_ID }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if(req.LegalDocumentID.IsEmpty() && req.ControlNo.IsEmpty())
                    {
                        req.LegalDocumentID = row["LGLDOC_ID"].Str();
                        req.ControlNo = row["CONTROLNO"].Str();
                        req.StatusRequestName = "Opened";
                        req.Status = "0";
                        req.ApplicationDate = DateTime.Now.ToString("MMM dd, yyyy");
                    }
                    return (Results.Success, "Succesfull save");
                }
                    
                else if (ResultCode == "2")
                    return (Results.Failed, "Legal Document O.R Number already Save.");
                else if (ResultCode == "0")
                    return (Results.Failed, "Check your Data, Please try again!");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object lgldoctrans)> Load_OtherDocumentRequest(LegalDocument_Transaction req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_LGLDOC0C1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmrequestor", account.USR_ID },
                {"parmrequeststatus", req.Status },
                {"parmrownum", req. num_row},
                {"parmsearch", req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetRequestBrgyOtherDocumentList(result.Read<dynamic>(), account.USR_ID, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object tpl)> Load_TemplateType()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_TPLTYP0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetTemplateTypeList(result.Read<dynamic>(), 1000));
            return (Results.Null, null);
        }

        public async Task<(Results result, object tpldoc)> Load_TemplateDoc(string templatetypeid)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_TPLDOC0B1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmtemplatetypeid", templatetypeid }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetTemplateDocList(result.Read<dynamic>(), 1000));
            return (Results.Null, null);
        }
    }
}
