﻿using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.Features.UserFeature;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.RequestModel.Feature;

namespace webapi.App.Aggregates.STLPartylistDashboard.Features
{
    [Service.ITransient(typeof(RequestDocumentRepository))]
    public interface IRequestDocumentRepository
    {
        Task<(Results result, String message, String reqdocid)> RequestDocumentAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid)> RequestBrgyDocumentAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid)> RequestBrgyClearanceAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid)> UpdateRequestDocumentAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid)> UpdateRequestBrgyClearanceAsync(RequestDocument request);
        Task<(Results result, String message, String reqdocid)> UpdateRequestBrgyDocumentAsync(RequestDocument request);
        Task<(Results result, object reqdoc)> LoadRequestDocument(FilterRequest request);
        Task<(Results result, object reqdoc)> LoadIssuesConcernAttachment(RequestDocument request);
        Task<(Results result, object doctype)> LoadDocumentType();
    }
    public class RequestDocumentRepository:IRequestDocumentRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public RequestDocumentRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, string reqdocid)> RequestDocumentAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmbizname", request.BusinessName},
                {"parmbizaddress",request.BusinessAddress },
                {"parmbizownername",request.BusinessOwnerName },
                {"parmbizowneraddress", request.BusinessOwnerAddress },
                {"parmbiztype", request.Type },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmcategory", request.CategoryID },
                {"parmxattchmnt",request.iAttachments },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["REQ_DOC_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "You already had request this document, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object reqdoc)> LoadRequestDocument(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_REQ_DOC0C01", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid", account.USR_ID },
                {"parmstatus", request.Status },
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllRequestDocumentList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, String reqdocid)> UpdateRequestDocumentAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0B", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid", request.ReqDocID },
                {"parmdoctypeid", request.DoctypeID },
                {"@parmcategorydocument", request.Category_Document },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmbizname", request.BusinessName},
                {"parmbizaddress",request.BusinessAddress },
                {"parmbizownername",request.BusinessOwnerName },
                {"parmbizowneraddress", request.BusinessOwnerAddress },
                {"parmbiztype", request.Type },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmcategory", request.CategoryID },
                {"parmattahcment",request.URLAttachment },
                {"parmctcno", request.CTCNo },
                {"parmorno", request.ORNO },
                {"parmamount", request.Amount },
                {"parmstatus", request.STATUS },
                {"parmxattchmnt",request.iAttachments },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save",request.ReqDocID);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "You already had request this document, Please try again", null);
            }
            return (Results.Null, null, null);
        }


        public async Task<(Results result, string message, string reqdocid)> RequestBrgyClearanceAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0E", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmpurpose", request.Purpose },
                {"parmcategory", request.CategoryID },
                {"parmattahcment",request.URLAttachment },
                {"parmxattchmnt",request.iAttachments },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                request.ReqDocID = row["REQ_DOC_ID"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", request.ReqDocID);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "2")
                    return (Results.Failed, "You already had request this document, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message, string reqdocid)> UpdateRequestBrgyClearanceAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0F", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid",request.ReqDocID },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmpurpose", request.Purpose },
                {"parmcategory", request.CategoryID },
                {"parmattahcment",request.URLAttachment },
                {"parmxattchmnt",request.iAttachments },
                {"parmctcno", request.CTCNo },
                {"parmorno", request.ORNO },
                {"parmamount", request.Amount },
                {"parmstatus", request.STATUS },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", request.ReqDocID);
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "3")
                    return (Results.Failed, "You already had request this document, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, object reqdoc)> LoadIssuesConcernAttachment(RequestDocument request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_REQDOCATTM", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid", request.ReqDocID}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAttachementReqDocAttmList(result.Read<dynamic>(), 100));
            return (Results.Null, null);
        }

        public async Task<(Results result, object doctype)> LoadDocumentType()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_DOCTYPE0D", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetDocumentTypeList(result.Read<dynamic>()));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message, string reqdocid)> RequestBrgyDocumentAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0A01", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmpurpose", request.Purpose },
                {"parmbizname", request.BusinessName},
                {"parmbizaddress",request.BusinessAddress },
                {"parmbizownername",request.BusinessOwnerName },
                {"parmbizowneraddress", request.BusinessOwnerAddress },
                {"parmbiztype", request.Type },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmcategory", request.CategoryID },
                {"parmotherdocument", request.OTRDocumentType },
                {"parmxattchmnt",request.iAttachments },
                {"parmisfree", request.isFree },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                request.ReqDocID = row["REQ_DOC_ID"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["REQ_DOC_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "2")
                    return (Results.Failed, "You already had request this document, Please try again", null);
            }
            return (Results.Null, null, null);
        }

        public async Task<(Results result, string message, string reqdocid)> UpdateRequestBrgyDocumentAsync(RequestDocument request)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_REQ_DOC0B01", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmreqdocid",request.ReqDocID },
                {"parmdoctypeid", request.DoctypeID },
                {"parmcategorydocument", request.Category_Document },
                {"parmpapplicationdate", request.ApplicationDate },
                {"parmpurpose", request.Purpose },
                {"parmbizname", request.BusinessName},
                {"parmbizaddress",request.BusinessAddress },
                {"parmbizownername",request.BusinessOwnerName },
                {"parmbizowneraddress", request.BusinessOwnerAddress },
                {"parmbiztype", request.Type },
                {"parmrequestorid", request.RequestorID },
                {"parmrequestorname", request.RequestorNM },
                {"parmcategory", request.CategoryID },
                {"parmotherdocument", request.OTRDocumentType },
                {"parmxattchmnt",request.iAttachments },
                {"parmisfree", request.isFree },
                {"parmoptrid",account.USR_ID },
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                request.ReqDocID = row["REQ_DOC_ID"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Successfully save", row["REQ_DOC_ID"].Str());
                else if (ResultCode == "0")
                    return (Results.Failed, "Check Details, Please try again", null);
                else if (ResultCode == "2")
                    return (Results.Failed, "You already had request this document, Please try again", null);
            }
            return (Results.Null, null, null);
        }
    }
}
