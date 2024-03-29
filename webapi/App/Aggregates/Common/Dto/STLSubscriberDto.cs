﻿using Comm.Commons.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Model.User;
using webapi.App.RequestModel.AppRecruiter;

namespace webapi.App.Aggregates.Common.Dto
{
    public class STLSubscriberDto
    {
        public static IDictionary<string, object> GetPartyList(IDictionary<string,object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PL_NM = data["PL_NM"].Str();
            o.PL_ADD = data["PL_ADD"].Str();
            o.PL_TEL_NO = data["PL_TEL_NO"].Str();
            o.URL_STRMNG_NM = data["URL_STRMNG_NM"].Str();
            o.URL_STRMNG = data["URL_STRMNG"].Str();
            return o;
        }
        public static IDictionary<string, object> GetAnnouncment(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.Title = data["NOTIF_TTL"].Str();
            o.Description = data["NOTIF_DESC"].Str();
            return o;
        }
        public static IDictionary<string, object> GetGroup(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.PSN_CD = data["PSN_CD"].Str();
            o.PLTCL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["PLTCL_NM"].Str()));
            return o;
        }



        public static IEnumerable<dynamic> GetAttachementIssuesConcernList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAttachementIssuesConcern_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAttachementIssuesConcern_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AttachementIssuesConcern_List(e));
        }
        public static IDictionary<string, object> Get_AttachementIssuesConcern_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["CompID"].Str();
            o.GroupID = data["GroupId"].Str();
            o.SequenceNo = data["SequenceNo"].Str();
            o.Attachment = data["Attachment"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetAttachementComplaintList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAttachementComplaint_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAttachementComplaint_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AttachementComplaint_List(e));
        }
        public static IDictionary<string, object> Get_AttachementComplaint_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["CompID"].Str();
            o.GroupID = data["GroupId"].Str();
            o.CaseNo = data["BRGY_CASE_NO"].Str();
            o.SequenceNo = data["SequenceNo"].Str();
            o.Attachment = data["Attachment"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetAllIssuesConcernList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllIssuesConcern_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllIssuesConcern_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllIssuesConcern_List(e));
        }
        public static IDictionary<string, object> Get_AllIssuesConcern_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["num_row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.Userid = data["USR_ID"].Str();
            o.Firstname = textInfo.ToTitleCase(data["FRST_NM"].Str());
            o.Lastname = textInfo.ToTitleCase(data["LST_NM"].Str());
            o.Middlename = textInfo.ToTitleCase(data["MDL_NM"].Str());
            o.Fullname = textInfo.ToTitleCase(data["FLL_NM"].Str());
            o.MobileNumber = data["MOB_NO"].Str();
            o.Sitio = data["LOC_SIT"].Str();
            o.SitioName = textInfo.ToTitleCase(data["SIT_NM"].Str());
            o.ImageUrl = data["IMG_URL"].Str();
            o.TransactionNo = data["TRN_NO"].Str();
            o.TicketNo = data["TCKT_NO"].Str();
            o.Subject = data["SBJCT"].Str();
            o.Body = data["BODY"].Str();
            o.STAT = data["STAT"].Str();
            o.CorrectiveAction = data["COR_ACTION"].Str();
            o.IssuedDate = (data["RGS_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["RGS_TRN_TS"].Str()).ToString("MMM dd, yyyy");
            o.ActionDate = (data["FXD_TRN_TS"].Str()=="") ? "" : Convert.ToDateTime(data["FXD_TRN_TS"].Str()).ToString("MMM dd, yyyy");
            o.TotalAttachment = Convert.ToInt32(data["TTL_ATTCHMNT"].Str());
            o.URLAttachment = data["ATTCHMNT"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }

        public static IEnumerable<dynamic> GetAllBrgyOfficialList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllBrgyOfficial_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllBrgyOfficial_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllBrgyOfficial_List(e));
        }
        public static IDictionary<string, object> Get_AllBrgyOfficial_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.BrgyOfficialID = data["BRGY_OFL_ID"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.BrgyCode = data["LOC_BRGY"].Str();
            o.Brgy = data["BRGY"].Str();
            o.SitioCode = data["LOC_SIT"].Str();
            o.Sitio = data["SIT_NM"].Str();
            o.BarangayPositionID = data["BRGY_PSTN_ID"].Str();
            o.BarangayPosition = data["POSITION"].Str();
            o.Category = data["CATEGORY"].Str();
            o.Sub_Category = data["SUB_CAT"].Str();
            o.ElectedOfficial = data["OFCL_NM"].Str();
            o.ResidentName = data["FLL_NM"].Str();
            o.Userid = data["USR_ID"].Str();
            o.RankNo = data["RNK_NO"].Str();
            o.Committee = data["CMTE"].Str();
            o.TermStart = (data["EF_DATE"].Str().IsEmpty()) ? "" : Convert.ToDateTime(data["EF_DATE"].Str()).ToString("MMM dd, yyyy");
            o.TermEnd = (data["EOT_DATE"].Str().IsEmpty()) ? "" : Convert.ToDateTime(data["EOT_DATE"].Str()).ToString("MMM dd, yyyy");
            o.ImageUrl = data["IMG_URL"].Str();
            o.SignatureURL = data["SIGNATUREID"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }
        public static IEnumerable<dynamic> GetAllComplaintList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllComplaint_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllComplaint_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllComplaint_List(e));
        }
        public static IDictionary<string, object> Get_AllComplaint_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ComplainantID = data["COMPLAINANT_ID"].Str();
            o.ComplainantName = data["COMPLAINANT_NM"].Str();
            o.Respondent = data["RESPONDENT_ID"].Str();
            o.RespondentName = data["RESPONDENT_NM"].Str();
            o.Witness = data["WTNS"].Str();
            o.IncidentPlace = data["INCIDENT_PLACE"].Str();
            o.IncidentPlaceName = data["SIT_NM"].Str();
            o.CaseNo = data["BRGY_CASE_NO"].Str();
            o.BrgyCaptain = data["BRGY_CPT"].Str();
            o.ComplaintType = data["CMPLNT_TYP"].Str();
            o.ComplaintTypeName = data["CMPLNT_TYP_NM"].Str();
            o.Issue = data["ACCUSATION"].Str();
            o.Statement = data["STATEMENT"].Str();
            o.IncidentDate = (data["INCIDENT_DT"].Str().IsEmpty()) ? "" : Convert.ToDateTime(data["INCIDENT_DT"].Str()).ToString("MMM dd, yyyy");
            o.IncidentTime = $"{Convert.ToDateTime(data["INCIDENT_DT"].Str()).ToString("MMM dd, yyyy")} {data["INCIDENT_TIME"].Str()}";
            o.IsComplaint = Convert.ToBoolean(data["S_CMPLNT"]);
            o.Status_Type = data["STS_TYP"];
            o.Status_Type_Name = data["STS_TYP_NM"];
            o.ComplaintCreatedBy = data["CMPLNT_CRT_BY"].Str();
            o.ComplaintCreatedDate = (data["CMPLNT_CRT_DT"].Str() == "") ? "" : Convert.ToDateTime(data["CMPLNT_CRT_DT"].Str()).ToString("MMM dd, yyyy");

            o.IsSummon = Convert.ToBoolean(data["IS_SUMMON"]);
            o.SummoCreatedBy = data["SUMMON_CREATED_BY"].Str();
            o.SummonCreatedDate = (data["SUMMON_CREATED_DT"].Str() == "") ? "" : Convert.ToDateTime(data["SUMMON_CREATED_DT"].Str()).ToString("MMM dd, yyyy");
            o.IsCancell = Convert.ToBoolean(data["S_CNCL"]);
            o.CancellBy = data["CNCL_BY"].Str();
            o.CancelledDate = (data["CNCL_DT"].Str() == "") ? "" : Convert.ToDateTime(data["CNCL_DT"].Str()).ToString("MMM dd, yyyy");
            o.IsRelease = Convert.ToBoolean(data["S_RSLV"]);
            o.ReleasedBy = data["RSLV_BY"].Str();
            o.ReleaseDate = (data["DT_RSLV"].Str() == "") ? "" : Convert.ToDateTime(data["DT_RSLV"].Str()).ToString("MMM dd, yyyy");
            o.TotalAttachment = Convert.ToInt32(data["TTL_ATTCHMNT"].Str());
            o.URLAttachment = data["ATTCHMNT"].Str();
            o.ImageUrl = data["IMG_URL"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetAllMemoList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllMemo_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllMemo_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllMemo_List(e));
        }
        public static IDictionary<string, object> Get_AllMemo_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.BrgyCode = data["LOC_BRGY"].Str();
            o.Brgy = data["BRGY"].Str();
            o.MemoID = data["MEMO_ID"].Str();
            o.MemorandumNo = data["MEMO_NO"].Str();
            o.Subject = textInfo.ToTitleCase(data["MEMO_SBJCT"].Str());
            o.MemoURL = data["MEMO_PATH"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }


        public static IEnumerable<dynamic> GetAllRespondentList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllRespondent_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllRespondent_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllRespondent_List(e));
        }
        public static IDictionary<string, object> Get_AllRespondent_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["num_row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.Username = data["USR_NM"].Str();
            o.RespondentID = data["USR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.AccountType = data["ACT_TYP"].Str();
            o.Firstname = textInfo.ToTitleCase(data["FRST_NM"].Str());
            o.Lastname = textInfo.ToTitleCase(data["LST_NM"].Str());
            o.Middlename = textInfo.ToTitleCase(data["MDL_NM"].Str());
            o.RespondentName = textInfo.ToTitleCase(data["FLL_NM"].Str());
            o.Nickname = textInfo.ToTitleCase(data["NCK_NM"].Str());
            o.PrecentNumber = data["PRCNT_NO"].Str();
            o.ClusterNumber = data["CLSTR_NO"].Str();
            o.MobileNumber = data["MOB_NO"].Str();
            o.EmailAddress = data["EML_ADD"].Str();
            o.HomeAddress = textInfo.ToUpper(data["HM_ADDR"].Str());
            o.PresentAddress = textInfo.ToUpper(data["PRSNT_ADDR"].Str());
            o.Region = data["LOC_REG"].Str();
            o.Province = data["LOC_PROV"].Str();
            o.Municipality = data["LOC_MUN"].Str();
            o.Barangay = data["LOC_BRGY"].Str();
            o.Sitio = data["LOC_SIT"].Str();
            o.SitioName = textInfo.ToTitleCase(data["SIT_NM"].Str());
            o.Gender = data["GNDR"].Str();
            o.MaritalStatus = data["MRTL_STAT"].Str();
            o.Citizenship = data["CTZNSHP"].Str();
            o.ImageUrl = data["IMG_URL"].Str();
            o.BirthDate = data["BRT_DT"].Str();
            o.BloodType = data["BLD_TYP"].Str();
            o.Nationality = textInfo.ToTitleCase(data["NATNLTY"].Str());
            o.Occupation = textInfo.ToTitleCase(data["OCCPTN"].Str());
            o.Skills = textInfo.ToTitleCase(data["SKLLS"].Str());
            o.PRF_PIC = data["PRF_PIC"].Str();
            o.S_BLCK = data["S_BLCK"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }

        public static IEnumerable<dynamic> GetAllMemberList(IEnumerable<dynamic> data, string userid="",int limit=100,bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllMember_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllMember_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllMember_List(e));
        }
        public static IDictionary<string, object> Get_AllMember_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["num_row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.Username = data["USR_NM"].Str();
            o.SUBSCR_TYP = data["SUBSCR_TYP"].Str();
            o.GroupRef = data["REF_GRP_ID"].Str();
            o.SiteLeader = data["REF_LDR_ID"].Str();
            o.SiteLeaderName = data["LDR_NM"].Str();
            o.LDR_TYP = data["LDR_TYP"].Str();
            o.LDR_TYP_NM = data["LDR_TYP_NM"].Str();
            o.LeaderType = data["LDR_TYP"].Str();
            o.Userid = data["SUBSCR_ID"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.AccountType = data["ACT_TYP"].Str();
            o.Firstname = textInfo.ToTitleCase(data["FRST_NM"].Str());
            o.Lastname = textInfo.ToTitleCase(data["LST_NM"].Str());
            o.Middlename = textInfo.ToTitleCase(data["MDL_NM"].Str());
            o.Fullname = textInfo.ToTitleCase(data["FLL_NM"].Str());
            o.Nickname = textInfo.ToTitleCase(data["NCK_NM"].Str());
            o.PrecentNumber = data["PRCNT_NO"].Str();
            o.ClusterNumber = data["CLSTR_NO"].Str();
            o.MobileNumber = data["MOB_NO"].Str();
            o.EmailAddress = data["EML_ADD"].Str();
            o.HomeAddress = textInfo.ToUpper(data["HM_ADDR"].Str());
            o.PresentAddress = textInfo.ToUpper(data["PRSNT_ADDR"].Str());
            o.Region = data["LOC_REG"].Str();
            o.Province = data["LOC_PROV"].Str();
            o.Municipality = data["LOC_MUN"].Str();
            o.Barangay = data["LOC_BRGY"].Str();
            o.Sitio = data["LOC_SIT"].Str();
            o.SitioName = textInfo.ToTitleCase(data["SIT_NM"].Str());
            o.Gender = data["GNDR"].Str();
            o.MaritalStatus = data["MRTL_STAT"].Str();
            o.Citizenship = data["CTZNSHP"].Str();
            o.ImageUrl = data["IMG_URL"].Str();
            o.BirthDate = data["BRT_DT"].Str();
            o.BloodType = data["BLD_TYP"].Str();
            o.Nationality = textInfo.ToTitleCase(data["NATNLTY"].Str());
            o.Occupation = textInfo.ToTitleCase(data["OCCPTN"].Str());
            o.Skills = textInfo.ToTitleCase(data["SKLLS"].Str());
            o.PRF_PIC = data["PRF_PIC"].Str();
            o.S_BLCK = data["S_BLCK"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }


        public static IEnumerable<dynamic> GetDonationList(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetDonation_List(data);
            return items;
        }
        public static IEnumerable<dynamic> GetDonation_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Donation_List(e));
        }
        public static IDictionary<string, object> Get_Donation_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.SUBSCR_ID = data["SUBSCR_ID"].Str();
            o.OTP_DT = data["OTP_DT"].Str();
            o.ACT_ID = data["ACT_ID"].Str();
            o.DONO_ID = data["DONO_ID"].Str();
            o.FLL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FLL_NM"].Str()));
            o.PUR = data["PUR"].Str();
            o.AMNT = Convert.ToDouble(data["AMNT"].Str());
            o.MOB_NO = data["MOB_NO"].Str();
            o.ImageUrl = data["IMG_URL"].Str();
            return o;
        }
        public static IEnumerable<dynamic> GetGroupList(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetGroup_List(data);
            return items;
        }
        public static IEnumerable<dynamic> GetGroup_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Group_List(e));
        }
        public static IDictionary<string, object> Get_Group_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.SUBSCR_ID = data["SUBSCR_ID"].Str();
            o.FLL_NM = textInfo.ToTitleCase(textInfo.ToLower(data["FLL_NM"].Str()));
            return o;
        }
        public static IEnumerable<dynamic> GetSitioList(IEnumerable<dynamic> data, string brgyid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetSitioLists(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.brgyid = brgyid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetSitioLists(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => GetSitio_List(e));
        }
        public static IDictionary<string, object> GetSitio_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.num_row = data["Num_Row"].Str();
            o.SIT_ID = data["SIT_ID"].Str();
            o.SIT_NM = data["SIT_NM"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetAllRequestDocumentList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllRequestDocument_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllRequestDocument_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllRequestDocument_List(e));
        }
        public static IDictionary<string, object> Get_AllRequestDocument_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ReqDocID = data["REQ_DOC_ID"].Str();
            o.DoctypeID = data["DOCTYP_ID"].Str();
            o.DoctypeNM = data["DOCTYP_NM"].Str();
            o.CategoryID = data["CATEGORY"].Str();
            o.Category_Doc_ID = data["CAT_DOC"].Str();
            o.Category_Document = data["CAT_DOC_NM"].Str();
            o.ApplicationDate = (data["APP_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyy");
            o.BusinessName = data["BIZ_NM"].Str();
            o.BusinessAddress = data["BIZ_ADDRESS"].Str();
            o.BusinessOwnerName = data["BIZ_OWNER"].Str();
            o.BusinessOwnerAddress = data["BIZ_OWNER_ADDRESS"].Str();
            o.Type = data["BIZ_TYP"].Str();
            o.RequestorID = data["REQTR_ID"].Str();
            o.RequestorNM = data["REQTR_NM"].Str();
            o.STATUS = data["STATUS"].Str();
            o.STATUS_NM = data["STATUS_NM"].Str();
            o.CategoryID = data["CATEGORY"].Str();
            o.CategoryNM = data["CATEGORY_NM"].Str();
            o.Purpose = data["PURPOSE"].Str();
            o.ORNO = data["OR_NO"].Str();
            o.CTCNo = data["CTC_NO"].Str();
            o.IssuedDate = (data["DATE_ISSUED"].Str() == "") ? "" : Convert.ToDateTime(data["DATE_ISSUED"].Str()).ToString("MMM dd, yyyy");
            o.Amount = data["AMOUNT"].Str();
            o.BrgyCaptain = data["BRGY_CAPTAIN"].Str();
            o.URL_DocPath = data["URL_DOCPATH"].Str();
            o.ControlNo = data["CNTRL_NO"].Str();
            o.Gender = data["GNDR"].Str();
            o.Gender_NM = data["GNDR_NM"].Str();
            o.MaritalStatus = data["MRTL_STAT"].Str();
            o.MaritalStatus_NM = data["MRTL_STAT_NM"].Str();
            o.Birthdate = data["BRT_DT"].Str();
            o.PurokSitio = data["SIT_NM"].Str();
            o.ProfilePicture = data["PRF_PIC"].Str();
            o.PresentAddress = textInfo.ToUpper(data["PRSNT_ADDR"].Str());
            o.MobileNo = data["MOB_NO"].Str();
            o.URLAttachment = data["URL_ATTACHMENT"].Str();
            o.TotalAttachment = (data["TTL_ATTM"].Str() == "") ? 0 : Convert.ToInt32(data["TTL_ATTM"].Str());
            o.OTRDocumentType = data["OTR_DOC_ID"].Str();
            o.OTRDocType = data["DOC_TYPE"].Str();
            o.OTRDocContent = data["DOC_Content"].Str();
            o.OTRCategory = data["OTR_CATEGORY"].Str();
            o.OTRCategoryNM = data["OTR_CATEGORY_NM"].Str();
            o.isFree = data["isFREE"].Str();
            o.isFreeNM = data["isFREE_NM"].Str();
            o.AppointmentDate = (data["APPT_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APPT_DATE"].Str()).ToString("MMM dd, yyyy");

            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }


        public static IEnumerable<dynamic> GetAllCommunityList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllCommunity_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllCommunity_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllCommunity_List(e));
        }
        public static IDictionary<string, object> Get_AllCommunity_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.CommunityID = data["COMM_ID"].Str();
            o.CommunityName = data["COMM_NM"].Str();
            o.CommunityDescription = data["COMM_DESC"].Str();
            o.TypeLevel = data["TYP_LVL"].Str();
            o.TypeLevelDescription = data["Typ_Level_Desc"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetAllPostCommunityList(IEnumerable<dynamic> data, string userid = "", int limit = 25, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllPostCommunity_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllPostCommunity_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllPostCommunity_List(e));
        }
        public static IDictionary<string, object> Get_AllPostCommunity_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PosterName = data["POSTER_NM"].Str();
            o.PosterImage = data["POSTER_IMG"].Str();
            o.PostID = data["POST_ID"].Str();
            o.PostTitle = data["POST_TTL"].Str();
            o.PostDescription = data["POST_DESC"].Str();
            o.isInActive = Convert.ToBoolean(data["isInActive"]);
            o.URL = data["IMG_CONTENT"].Str();
            o.CommunityID = data["COMM_ID"].Str();
            o.PosterID = data["USR_ID"].Str();
            o.isLeave = Convert.ToBoolean(data["isLEAVE"]);
            o.CommunityName = data["COMM_NM"].Str();
            o.CommunityDescription = data["COMM_DESC"].Str();
            o.Post_Date = data["UPD_DT"].Str();
            o.Total_Comment = data["Total_Comment"].Str();
            //o.Total_CommenAb = GetTotalAbbreviation(999); ;
            o.isLike = Convert.ToInt32(data["isLike"]);
            o.isDisLike = Convert.ToInt32(data["isDisLike"]);
            o.Total_Like = data["Total_Like"].Str();
            //o.Total_LikeAb = GetTotalAbbreviation(Convert.ToDouble(data["Total_Like"]));
            //o.Total_LikeAb = GetTotalAbbreviation(999999999);
            o.Total_Dislike = data["Total_disLike"].Str();
            //o.Total_DislikeAb = GetTotalAbbreviation(Convert.ToDouble(data["Total_disLike"]));
            //o.Total_DislikeAb = GetTotalAbbreviation(999999);
            return o;
        }
        public static string GetTotalAbbreviation(double total)
        {
            string strtotal = "0";
            if (total >= 0 && total <= 999)
                strtotal = total.Str();
            else if(total > 999 && total <= 999999)
            {
                if(total % 1000 == 0)
                    strtotal = (total / 1000).Str() + "K";
                else
                {
                    int htotal = Convert.ToInt32(total) % 1000;
                    if(htotal % 100 == 0)
                    {
                        strtotal = (Convert.ToInt32(total) / 1000).Str() + "."+ (Convert.ToInt32(htotal) /100) +"K";
                    }
                    else
                    {
                        strtotal = (Convert.ToInt32(total) / 1000).Str() + "." + (Convert.ToInt32(htotal) / 100) + "K+";
                    }
                }
            }
            else if(total > 999999 && total <= 999999999)
            {
                if(total % 1000000 == 0)
                {
                    strtotal = (Convert.ToInt32(total) / 1000000).Str() + "M";
                }
                else
                {
                    int ktotal = Convert.ToInt32(total) % 1000000;
                    if(ktotal % 1000 == 0)
                    {
                        strtotal = (Convert.ToInt32(total) / 1000000).Str() + "." + (ktotal / 1000).Str() + "M";
                    }
                    else
                    {
                        strtotal = (Convert.ToInt32(total) / 1000000).Str() + "." + (ktotal / 10000).Str() + "M+";
                    }
                }
            }
            return strtotal;
        }

        public static IEnumerable<dynamic> GetAllCommentPostCommunityList(IEnumerable<dynamic> data, string userid = "", int limit = 30, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllCommentPostCommunity_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllCommentPostCommunity_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllCommentPostCommunity_List(e));
        }
        public static IDictionary<string, object> Get_AllCommentPostCommunity_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.CommenterName = data["CommenterName"].Str();
            o.CommenterMobileNumber = data["CommenterMobileNumber"].Str();
            o.CommenterImage = data["CommenterImage"].Str();
            o.CommunityID = data["COMM_ID"].Str();
            o.PostID = data["POST_ID"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.CommentID = data["CMN_ID"].Str();
            o.CommentLevel = data["COMN_LVL"].Str();
            o.CommentDescription = data["CMN_DESCRIPTION"].Str();
            o.isRemove = Convert.ToInt32(data["isREMOVE"]);
            o.Reason = data["Reason"].Str();
            o.isYou = Convert.ToInt32(data["isYou"]);
            o.isViewRemoveComment = Convert.ToInt32(data["isViewRemoveComment"]);
            o.Total_Like = Convert.ToInt32(data["Total_Like"]);
            o.Total_disLike = Convert.ToInt32(data["Total_disLike"]);
            o.isLike = Convert.ToInt32(data["isLike"]);
            o.isDisLike = Convert.ToInt32(data["isDisLike"]);
            o.Post_Date = data["Post_Date"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetAllCommunitiesList(IEnumerable<dynamic> data, string userid = "", int limit = 30, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllCommunities_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllCommunities_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllCommunities_List(e));
        }
        public static IDictionary<string, object> Get_AllCommunities_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.CommunityID = data["COMM_ID"].Str();
            o.CommunityName = data["COMM_NM"].Str();
            o.CommunityDescription = data["COMM_DESC"].Str();
            o.CommunityPostCount = data["POST_COUNT"].Str();
            o.CommunityJoinCount = data["JOINERS_COUNT"].Str();
            o.CommunityTypeLevel = data["TYP_LVL"].Str();
            o.CommunityTypeLevelDescription = data["Typ_Level_Desc"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetAllContactList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllContact_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //    filter.Userid = userid;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetAllContact_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllContact_List(e));
        }
        public static IDictionary<string, object> Get_AllContact_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.displayFirstCharacter = data["displayFirstCharacter"].Str();
            o.RequestID = data["USR_ID"].Str();
            o.displayName = data["displayName"].Str();
            o.MobileNumbers = data["MobileNumbers"].Str();
            o.isBIMSS = Convert.ToBoolean(data["isBIMSS"]);
            o.isConnected = Convert.ToBoolean(data["IS_CNCTD"]);
            o.isAccepted = Convert.ToBoolean(data["IS_ACPTD"]);
            o.isActive = Convert.ToBoolean(data["IS_ACTV"]);
            o.isDeclined = Convert.ToBoolean(data["IS_DCLND"]);
            o.isCanceled = Convert.ToBoolean(data["IS_CNCLD"]);
            o.ImageUrl = data["IMG_URL"].Str();

            
            return o;
        }

        public static IEnumerable<dynamic> GetAttachementReqDocAttmList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAttachementReqDocAttm_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAttachementReqDocAttm_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AttachementReqDocAttm_List(e));
        }
        public static IDictionary<string, object> Get_AttachementReqDocAttm_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["CompID"].Str();
            o.GroupID = data["GroupId"].Str();
            o.SequenceNo = data["SequenceNo"].Str();
            o.AttachmentNo = data["AttachmentNo"].Str();
            o.RequestDocumentID = data["RequestDocumentID"].Str();
            o.Attachment = data["Attachment"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetDocumentTypeList(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetDocumentType_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetDocumentType_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_DocumentType_List(e));
        }
        public static IDictionary<string, object> Get_DocumentType_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["PL_ID"].Str();
            o.GroupID = data["PGRP_ID"].Str();
            o.BrgyCOde = data["LOC_BRGY"].Str();
            o.BarangayNM = data["BRGY"].Str();
            o.DocumentTypeID = data["DOC_TYP_ID"].Str();
            o.DocumentType = data["DOC_TYPE"].Str();
            o.Category = data["CATEGORY"].Str();
            o.CategoryNM = data["CATEGORY_NM"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetBrgyList(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetBrgyLists(data);
            return items;
        }
        public static IEnumerable<dynamic> GetBrgyLists(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => GetBrgy_List(e));
        }
        public static IDictionary<string, object> GetBrgy_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.Code = data["BRGY_CODE"].Str();
            o.Name = data["BRGY"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetReligionList(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetReligion_List(data);
            return items;
        }
        public static IEnumerable<dynamic> GetReligion_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Religion_List(e));
        }
        public static IDictionary<string, object> Get_Religion_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.num_row = data["Num_row"].Str();
            o.Religion = data["Religion"].Str();
            o.ReligionName = data["ReligionName"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetRequstCedula_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = data.Select(e => DesirializeCedulaRequest(e));
            return items;
        }

        public static IDictionary<string, object> DesirializeCedulaRequest(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.RequestId = data["CTC_ID"].Str();
            o.AmountPaid = data["TTL_AMT_PD"].Str();
            o.isClaimable = data["CLM_DT"].Str() == "" ? false : (DateTime.Parse(data["CLM_DT"].Str()).Date == DateTime.Now.Date);
            o.isClaimed = Int64.Parse(data["S_CLM"].Str()) == 1 ? true : false;
            o.ReleaseDate = data["RLS_DT"].Str() == "" ? null : DateTime.Parse(data["RLS_DT"].Str()).ToString("MM/dd/yyyy");
            o.CancelledDate = data["CNCL_DT"].Str() == "" ? null : DateTime.Parse(data["CNCL_DT"].Str()).ToString("MM/dd/yyyy");
            o.isCanceled = Int64.Parse(data["S_CNCL"].Str()) == 1 ? true : false;
            o.RequestDate = data["RQ_DT"].Str() == "" ? null : DateTime.Parse(data["RQ_DT"].Str()).ToString("MM/dd/yyyy");
            return o;
        }

        public static IEnumerable<dynamic> GetEmergency_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = data.Select(e => DesirializeEmergencyList(e));
            return items;
        }

        public static IDictionary<string, object> DesirializeEmergencyList(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.EmergencyId = data["EMGY_TYP_ID"].Str();
            o.EmergencyName = data["EMERGENCY_TYPE"].Str();
            o.EmergencyMessage = data["TEXT_MESSAGE"].Str();
            return o;
        }

        public static string GetSeries(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = data.Select(e => DesirializeSeries(e)).FirstOrDefault().Series;
            return items;
        }

        public static IDictionary<string, object> DesirializeSeries(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            o.Series = data["SERIES"].Str().PadLeft(5,'0');
            return o;
        }

        public static IEnumerable<dynamic> GetAllRegisterBusinessList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllRegisterBusiness_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //    filter.Userid = userid;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetAllRegisterBusiness_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllRegisterBusiness_List(e));
        }
        public static IDictionary<string, object> Get_AllRegisterBusiness_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.BusinessID = data["BIZ_ID"].Str();
            //o.Category = data["CATEGORY"].Str();
            o.BusinessControlNo = data["CTRL_NO"].Str();
            //o.CTCNo = data["CTC_NO"].Str();
            //o.InitialCapital = data["INIT_CAP"].Str();
            o.RegisteredNo = textInfo.ToUpper(textInfo.ToLower(data["REG_NO"].Str()));
            o.NatureofBusiness = textInfo.ToUpper(textInfo.ToLower(data["NTREBIZ"].Str()));
            o.BusinessName = textInfo.ToUpper(textInfo.ToLower(data["BIZ_NM"].Str()));
            o.BusinessEmail = data["BIZ_EMAIL"].Str();
            o.BusinessContactNo = data["CT_NO"].Str();
            o.BusinessAddress = textInfo.ToUpper(textInfo.ToLower(data["BIZ_ADDR"].Str()));
            o.BusinessDateOperate = data["DATE_OPRT"].Str();
            o.BusinessOwnershipTypeID = data["OWNRSHP_TYP"].Str();
            o.BusinessOwnershipTypeNM = textInfo.ToUpper(textInfo.ToLower(data["OWNRSHP_TYP_NM"].Str()));
            o.isInActive = Convert.ToBoolean(data["nSTATUS"].Str());
            o.Owner_ID = data["OWN_ID"].Str();
            o.Owner_NM = textInfo.ToUpper(textInfo.ToLower(data["OWNR_NM"].Str()));
            o.OwnerAddres = textInfo.ToUpper(textInfo.ToLower(data["OWNR_ADDRESS"].Str()));
            o.Owner_ContactNo = data["MOB_NO"].Str();
            o.Owner_Email = data["EML_ADD"].Str();

            //o.FirstName = data["OWN_FRST_NM"].Str();
            //o.MiddleInitial = data["OWN_MI_NM"].Str();
            //o.LastName = data["OWN_LST_NM"].Str();
            //o.FullName = data["OWN_FLL_NM"].Str();
            //o.OwnerAddress = data["OWN_ADDR"].Str();
            //o.OwnerEmail = data["OWN_EMAIL"].Str();
            //o.OwnerContactNo = data["OWN_CT_NO"].Str();
            o.Request = (data["REQ_DOC"].Str() == "") ? 0 : Convert.ToInt32(data["REQ_DOC"].Str());

            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }



        public static IEnumerable<dynamic> GetBrygBusinessClearanceList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetBrygBusinessClearance_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetBrygBusinessClearance_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_BrygBusinessClearance_List(e));
        }
        public static IDictionary<string, object> Get_BrygBusinessClearance_List(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.BusinessClearanceID = data["BIZCLR_ID"].Str();
            o.ControlNo = data["CTNRL_NO"].Str();
            o.BusinessID = data["BIZ_ID"].Str();
            o.BusinessNM = textInfo.ToUpper(textInfo.ToLower(data["BIZ_NM"].Str()));
            o.BusinessAddress = textInfo.ToUpper(textInfo.ToLower(data["BIZ_ADDR"].Str()));
            o.DateOperate = data["DATE_OPRT"].Str();
            o.ProfilePicture = data["PRF_PIC"].Str();
            o.OwnerID = data["OWNER_ID"].Str();
            o.OwnerNM = textInfo.ToUpper(textInfo.ToLower(data["OWNER_NM"].Str()));
            o.MobileNo = data["MobileNo"].Str();
            o.Birthdate = (data["BRT_DT"].Str() == "") ? "" : Convert.ToDateTime(data["BRT_DT"].Str()).ToString("MMM dd, yyyy");
            o.OwnerAddress = textInfo.ToUpper(textInfo.ToLower(data["OWNR_ADDRESS"].Str()));
            o.DateIssued = (data["DATEISSUED"].Str() == "") ? "" : Convert.ToDateTime(data["DATEISSUED"].Str()).ToString("MMM dd, yyyy");
            o.ExpiryDate = (data["EXPIREDDATE"].Str() == "") ? "" : Convert.ToDateTime(data["EXPIREDDATE"].Str()).ToString("MMM dd, yyyy");

            o.ORNumber = textInfo.ToUpper(textInfo.ToLower(data["OR_NO"].Str()));
            //o.AmountPaid = Convert.ToDecimal(string.Format("{0:#.0}", data["AMOUNT_PAID"].Str()));
            o.AmountPaid = (data["AMOUNT_PAID"].Str() == "") ? "0" : Convert.ToDouble(data["AMOUNT_PAID"].Str()).ToString("n2");
            o.DocStamp = (data["DOC_STAMP"].Str() == "") ? "0" : Convert.ToDecimal(data["DOC_STAMP"].Str()).ToString("n2");
            o.TotalAmount = (data["TTL_AMNT"].Str() == "") ? "0" : Convert.ToDecimal(data["TTL_AMNT"].Str()).ToString("n2");

            o.EnableCommunityTax = Convert.ToBoolean(data["ENABLECTC"].Str());
            o.CTCNo = textInfo.ToUpper(textInfo.ToLower(data["CTC"].Str()));
            o.CTCIssuedAt = textInfo.ToUpper(textInfo.ToLower(data["CTCISSUEDAT"].Str()));
            o.CTCIssuedOn = (data["CTCISSUEDON"].Str() == "") ? "" : Convert.ToDateTime(data["CTCISSUEDON"].Str()).ToString("MMM dd, yyyy");

            o.VerifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["VERIFIEDBY"].Str()));
            o.CertifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["CERTIFIEDBY"].Str()));
            o.StatusRequest = data["STAT_REQ"].Str();
            o.URLDoc = data["URL_DOCPATH"].Str();

            o.Release = (data["RELEASED"].Str()=="") ? false : Convert.ToBoolean(data["RELEASED"].Str());
            o.MosValidity = (data["MOS_VAL"].Str() == "") ? 0 : Convert.ToInt32(data["MOS_VAL"].Str());
            o.IssuedDate = (data["DATEPROCESS"].Str() == "") ? "" : Convert.ToDateTime(data["DATEPROCESS"].Str()).ToString("MMM dd, yyyy");
            o.DateRelease = (data["DATERELEASED"].Str() == "") ? "" : Convert.ToDateTime(data["DATERELEASED"].Str()).ToString("MMM dd, yyyy");
            o.ExpiryDate = (data["VALIDITYDATE"].Str() == "") ? "" : Convert.ToDateTime(data["VALIDITYDATE"].Str()).ToString("MMM dd, yyyy");
            o.ReleasedBy = textInfo.ToUpper(textInfo.ToLower(data["RELEASEDBY"].Str()));
            o.Cancelled = (data["CANCELLED"].Str() == "") ? false : Convert.ToBoolean(data["CANCELLED"].Str());
            o.CancelledBy = textInfo.ToUpper(textInfo.ToLower(data["CANCELLEDBY"].Str()));
            o.CancelledDate = (data["DATECANCELLED"].Str() == "") ? "" : Convert.ToDateTime(data["DATECANCELLED"].Str()).ToString("MMM dd, yyyy");
            o.AppointmentDate = (data["APP_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyy");
            o.ApplicationDate = (data["RGS_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["RGS_TRN_TS"].Str()).ToString("MMM dd, yyyy");
            return o;
        }



        public static IEnumerable<dynamic> GetBrygClearanceList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetBrygClearance_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetBrygClearance_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_BrygClearance_List(e));
        }
        public static IDictionary<string, object> Get_BrygClearance_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            string strnum_row = data["Num_Row"].Str();
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ClearanceNo = data["BRGYCLR_ID"].Str();
            o.ControlNo = data["CNTRL_NO"].Str();
            o.UserID = data["USR_ID"].Str();
            o.ResidentIDNo = data["RESIDENT_ID_NO"].Str();
            o.Requestor = textInfo.ToUpper(textInfo.ToLower(data["FLL_NM"].Str()));
            o.MobileNo = data["MOB_NO"].Str();
            o.Birthdate = (data["BRT_DT"].Str() == "") ? "" : Convert.ToDateTime(data["BRT_DT"].Str()).ToString("MMM dd, yyyy");
            o.TypeofClearance = data["CERTTYP_ID"].Str();
            //o.TypeofClearanceNM = textInfo.ToUpper(textInfo.ToLower(data["CERTTYP_NM"].Str()));
            o.TypeofClearanceNM = data["CERTTYP_NM"].Str();
            o.PurposeID = data["PURP_ID"].Str();
            o.PurposeNM = data["PURP_NM"].Str();
            o.ProfilePicture = data["PRF_PIC"].Str();
            o.URLDoc = data["URL_DOCPATH"].Str();

            o.ORNumber = textInfo.ToUpper(textInfo.ToLower(data["OR_NO"].Str()));
            //o.AmountPaid = Convert.ToDecimal(string.Format("{0:#.0}", data["AMOUNT_PAID"].Str()));
            o.AmountPaid = Convert.ToDouble(data["AMOUNT_PAID"].Str()).ToString("n2");
            o.DocStamp = Convert.ToDecimal(data["DOC_STAMP"].Str()).ToString("n2");
            o.TotalAmount = Convert.ToDecimal(data["TTL_AMNT"].Str()).ToString("n2");

            o.EnableCommunityTax = Convert.ToBoolean(data["ENABLECTC"].Str());
            o.CTCNo = textInfo.ToUpper(textInfo.ToLower(data["CTC"].Str()));
            o.CTCIssuedAt = textInfo.ToUpper(textInfo.ToLower(data["CTCISSUEDAT"].Str()));
            o.CTCIssuedOn = (data["CTCISSUEDON"].Str() == "") ? "" : Convert.ToDateTime(data["CTCISSUEDON"].Str()).ToString("MMM dd, yyyy");

            o.VerifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["VERIFIEDBY"].Str()));
            o.CertifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["CERTIFIEDBY"].Str()));

            o.Release = Convert.ToBoolean(data["RELEASED"].Str());
            o.MosValidity = (data["MOS_VAL"].Str() == "") ? 0 : Convert.ToInt32(data["MOS_VAL"].Str());
            o.IssuedDate = (data["DATEPROCESS"].Str() == "") ? "" : Convert.ToDateTime(data["DATEPROCESS"].Str()).ToString("MMM dd, yyyy");
            o.DateRelease = (data["DATERELEASED"].Str() == "") ? "" : Convert.ToDateTime(data["DATERELEASED"].Str()).ToString("MMM dd, yyyy");
            o.ExpiryDate = (data["VALIDITYDATE"].Str() == "") ? "" : Convert.ToDateTime(data["VALIDITYDATE"].Str()).ToString("MMM dd, yyyy");
            o.ReleasedBy = textInfo.ToUpper(textInfo.ToLower(data["RELEASEDBY"].Str()));
            o.Cancelled = Convert.ToBoolean(data["CANCELLED"].Str());
            o.StatusRequest = data["STAT_REQ"].Str();
            o.CancelledBy = textInfo.ToUpper(textInfo.ToLower(data["CANCELLEDBY"].Str()));
            o.CancelledDate = (data["DATECANCELLED"].Str() == "") ? "" : Convert.ToDateTime(data["DATECANCELLED"].Str()).ToString("MMM dd, yyyy");
            o.AppointmentDate = (data["APP_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyy");
            o.ApplicationDate = (data["RGS_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["RGS_TRN_TS"].Str()).ToString("MMM dd, yyyy");
            return o;
        }


        public static IEnumerable<dynamic> GetNewsList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetNews_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetNews_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_News_List(e));
        }
        public static IDictionary<string, object> Get_News_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            string strnum_row = data["Num_Row"].Str();
            //string s1 = (data["category"].Str()).Substring(0, 1);
            //string s2 = textInfo.ToLower(data["category"].Str()).Remove(0, 1);
            o.num_row = data["Num_Row"].Str();
            o.category = textInfo.ToTitleCase(data["category"].Str());
            o.source_name = data["name"].Str();
            o.source_id = data["id"].Str();
            o.author = data["author"].Str();
            o.title = data["title"].Str();
            o.description = data["description"].Str();
            o.url = data["url"].Str();
            o.urlToImage = data["urlToImage"].Str();
            o.publishedAt = data["publishedAt"].Str();
            o.publishedDate = (data["publishedDate"].Str() == "") ? "" : Convert.ToDateTime(data["publishedDate"].Str()).ToString("MMM dd, yyyy");
            o.news_content = data["news_content"].Str();

            return o;
        }


        public static IEnumerable<dynamic> GetEducationAttainmentList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetEducationAttainment_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetEducationAttainment_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_EducationAttainment_List(e));
        }
        public static IDictionary<string, object> Get_EducationAttainment_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.SEQ_NO = Convert.ToInt32(data["SEQ_NO"].Str());
            o.EducLevel = data["SCH_LVL"].Str();
            o.School = data["SCH_NM"].Str();
            o.SchoolAddress = data["SCH_ADDRESS"].Str();
            o.SchoolYear = data["SCH_YEAR"].Str();
            o.Course = data["SCH_COURSE"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetNewsCategoryList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetNewsCategory_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetNewsCategory_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_NewsCategory_List(e));
        }
        public static IDictionary<string, object> Get_NewsCategory_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.CategoryName = textInfo.ToTitleCase(data["category"].Str());
            return o;
        }

        public static IEnumerable<dynamic> GetEmploymentHistoryList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetEmploymentHistory_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetEmploymentHistory_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_EmploymentHistory_List(e));
        }
        public static IDictionary<string, object> Get_EmploymentHistory_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.SEQ_NO = Convert.ToInt32(data["SEQ_NO"].Str());
            o.Company = data["COMPANY"].Str();
            o.CompanyAddress = data["COMPANY_ADDRESS"].Str();
            o.RenderedFrom = data["RENERED_FROM"].Str();
            o.RenderedTo = data["RENERED_TO"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetSearchedResidentsList(IEnumerable<dynamic> data, string userid = "")
        {
            if (data == null) return null;
            var items = GetSearchedResidents_List(data);
            return items;

        }
        public static IEnumerable<dynamic> GetSearchedResidents_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_SearchedResidents_List(e));
        }
        public static IDictionary<string, object> Get_SearchedResidents_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.UserId = data["USR_ID"].Str();
            o.FullName = textInfo.ToTitleCase(data["FLL_NM"].Str().ToLower());
            o.Gender = data["GNDR"].Str();
            o.ProfilePic = data["PRF_PIC"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetFamilyList(IEnumerable<dynamic> data, string userid = "")
        {
            if (data == null) return null;
            var items = GetFamily_List(data);
            return items;

        }
        public static IEnumerable<dynamic> GetFamily_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Family_List(e));
        }
        public static IDictionary<string, object> Get_Family_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.FamilyId = data["FAM_ID"].Str();
            o.UserId = data["USR_ID"].Str();
            o.MemberId = data["MBR_ID"].Str();
            o.Name = data["FLL_NM"].Str();
            o.Gender = data["GNDR"].Str();
            o.Relationship = data["RLTSP"].Str();
            o.ProfilePic = data["PRF_PIC"].Str();
            return o;
        }





        public static IEnumerable<dynamic> GetOrganizationHistoryList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetOrganizationHistory_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetOrganizationHistory_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_OrganizationHistory_List(e));
        }
        public static IDictionary<string, object> Get_OrganizationHistory_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.USR_ID = data["USR_ID"].Str();
            o.SEQ_NO = Convert.ToInt32(data["SEQ_NO"].Str());
            o.OrganizationID = data["OrganizationID"].Str();
            o.OrganizationName = data["OrganizationName"].Str();
            o.OrganizationAbbr = data["OrganizatioAbbr"].Str();
            o.EstablishedDate = data["EstablishedDate"].Str();
            return o;
        }
        public static IEnumerable<dynamic> GetAllGovenmentIDList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllGovenmentID_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllGovenmentID_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllGovenmentID_List(e));
        }
        public static IDictionary<string, object> Get_AllGovenmentID_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.Num_Row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.GovernmentID = data["ID"].Str();
            o.GovernmentID_NM = data["GOVERNMENTID"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetAllEstablishmentList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllEstablishment_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllEstablishment_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllEstablishment_List(e));
        }
        public static IDictionary<string, object> Get_AllEstablishment_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.Est_ID = data["ESTID"].Str();
            o.Est_Name = data["EST_NM"].Str();
            o.Est_Type = data["EST_TYP"].Str();
            o.ContactDetails = data["CON_DET"].Str();
            o.Address = data["EST_ADR"].Str();
            o.EmailAddress = data["EMAIL_ADR"].Str();
            o.CompanyLogo = (data["LOGO"].Str()).Replace("www.", "");
            o.EstablishmentLocation = data["URL_LINKED"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetAllEmergencyAlertList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllEmergencyAlert_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllEmergencyAlert_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllEmergencyAlert_List(e));
        }
        public static IDictionary<string, object> Get_AllEmergencyAlert_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.EmgyID = data["EMGY_ID"].Str();
            o.EmgyTypID = data["EMGY_TYP_ID"].Str();
            o.EmgyType = data["EMERGENCY_TYPE"].Str();
            o.Message = data["TEXT_MESSAGE"].Str();
            o.UserID = data["USR_ID"].Str();
            o.FUllName = data["FLL_NM"].Str();
            o.AlertDate = data["ALRT_DATE"].Str();
            o.GeolocationLat = data["GEO_LOC_LAT"].Str();
            o.GeolocationLong = data["GEO_LOC_LONG"].Str();
            o.Closed_Details = data["CLOSED_DETAILS"].Str();
            o.ClosedDate = data["CLOSED_DATE"].Str();
            o.Closed_Type = Convert.ToInt32(data["CLOSED_TYP"]);
            o.Closed_TypeName = data["CLOSED_TYP_NM"].Str();
            return o;
        }



        public static IEnumerable<dynamic> GetOrganizationList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetOrganization_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetOrganization_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Organization_List(e));
        }

        public static IDictionary<string, object> Get_Organization_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.OrganizationID = data["OrganizationID"].Str();
            o.OrganizationName = data["OrganizationName"].Str();
            o.OrganizationAbbr = data["OrganizatioAbbr"].Str();
            o.EstablishedDate = data["EstablishedDate"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetGovernmentIssuedList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetGovernmentIssued_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetGovernmentIssued_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_GovernmentIssued_List(e));
        }

        public static IDictionary<string, object> Get_GovernmentIssued_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.GovernmentID = data["GOVVAL_ID"].Str();
            o.GovernmentID_NM = data["GOVERNMENTID"].Str();
            o.GovValIDNo = data["GOVVAL_ID_NO"].Str();
            o.ImageUrl = data["ATTACHMENT"].Str();
            o.Attachment = data["ATTACHMENT"].Str();
            return o;
        }



        public static IEnumerable<dynamic> GetRequestBrgyOtherDocumentList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetRequestBrgyOtherDocument_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;

        }
        public static IEnumerable<dynamic> GetRequestBrgyOtherDocument_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_RequestBrgyOtherDocument_List(e));
        }
        public static IDictionary<string, object> Get_RequestBrgyOtherDocument_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.LegalDocumentID = data["LGLDOC_ID"].Str();
            o.ControlNo = data["CONTROLNO"].Str();
            o.TemplateID = data["TPL_ID"].Str();
            o.TemplateName = data["TPL_NM"].Str();
            o.TemplateTypeID = data["TPLTYP_ID"].Str();
            o.TemplateTypeName = data["TPLTTYP_NM"].Str();
            o.UserID = data["REQUESTORID"].Str();
            o.RequestorNM = textInfo.ToUpper(textInfo.ToLower(data["REQUESTORNM"].Str()));
            o.MobileNo = data["MOB_NO"].Str();
            o.Birthdate = (data["BRT_DT"].Str() == "") ? "" : Convert.ToDateTime(data["BRT_DT"].Str()).ToString("MMM dd, yyyy");
            o.ProfilePicture = data["PRF_PIC"].Str();
            o.URLDoc = data["URL_DOCPATH"].Str();

            o.ORNumber = textInfo.ToUpper(textInfo.ToLower(data["OR_NO"].Str()));
            o.AmountPaid = (data["AMOUNT_PAID"].Str() == "") ? Convert.ToDouble(0).ToString("n2") : Convert.ToDouble(data["AMOUNT_PAID"].Str()).ToString("n2");
            o.DocStamp = (data["DOC_STAMP"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDecimal(data["DOC_STAMP"].Str()).ToString("n2");
            o.TotalAmount = (data["TTL_AMNT"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDecimal(data["TTL_AMNT"].Str()).ToString("n2");

            o.EnableCommunityTax = (data["ENABLECTC"].Str() == "") ? false : Convert.ToBoolean(data["ENABLECTC"].Str());
            o.CTCNo = textInfo.ToUpper(textInfo.ToLower(data["CTC"].Str()));
            o.CTCIssuedAt = textInfo.ToUpper(textInfo.ToLower(data["CTCISSUEDAT"].Str()));
            o.CTCIssuedOn = (data["CTCISSUEDON"].Str() == "") ? "" : Convert.ToDateTime(data["CTCISSUEDON"].Str()).ToString("MMM dd, yyyy");

            o.VerifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["VERIFIEDBY"].Str()));
            o.CertifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["CERTIFIEDBY"].Str()));

            o.Release = (data["RELEASED"].Str() == "") ? false : Convert.ToBoolean(data["RELEASED"].Str());
            o.ReleasedBy = textInfo.ToUpper(textInfo.ToLower(data["RELEASEDBY"].Str()));
            o.DateRelease = (data["DATERELEASED"].Str() == "") ? "" : Convert.ToDateTime(data["DATERELEASED"].Str()).ToString("MMM dd, yyyy");
            o.IssuedDate = (data["DATEPROCESS"].Str() == "") ? "" : Convert.ToDateTime(data["DATEPROCESS"].Str()).ToString("MMM dd, yyyy");
            o.MosValidity = (data["MOS_VAL"].Str() == "") ? 0 : Convert.ToInt32(data["MOS_VAL"].Str());
            o.ExpiryDate = (data["VALIDITYDATE"].Str() == "") ? "" : Convert.ToDateTime(data["VALIDITYDATE"].Str()).ToString("MMM dd, yyyy");

            o.Status = data["STAT_REQ"].Str();
            o.StatusRequestName = data["STAT_REQ_NM"].Str();

            o.Cancelled = (data["CANCELLED"].Str() == "") ? false : Convert.ToBoolean(data["CANCELLED"].Str());
            o.CancelledBy = textInfo.ToUpper(textInfo.ToLower(data["CANCELLEDBY"].Str()));
            o.CancelledDate = (data["DATECANCELLED"].Str() == "") ? "" : Convert.ToDateTime(data["DATECANCELLED"].Str()).ToString("MMM dd, yyyy");
            o.AppointmentDate = (data["APP_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyy");
            o.ApplicationDate = (data["RGS_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["RGS_TRN_TS"].Str()).ToString("MMM dd, yyyy");
            return o;
        }

        public static IEnumerable<dynamic> GetCertificateTypeList(IEnumerable<dynamic> data, int limit = 1000, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetCertificateType_List(data);
            var count = items.Count();
            return items;
        }
        public static IEnumerable<dynamic> GetCertificateType_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_CertificateType_List(e));
        }
        public static IDictionary<string, object> Get_CertificateType_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["PL_ID"].Str();
            o.PGRPID = data["PGRP_ID"].Str();
            o.TypeofClearance = data["CERTTYP_ID"].Str();
            o.TypeofClearanceNM = data["CertificatType"].Str();
            return o;
        }





        public static IEnumerable<dynamic> GetBrgyOperatorList(IEnumerable<dynamic> data, int limit = 1000, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetBrgyOperator_List(data);
            var count = items.Count();
            return items;
        }
        public static IEnumerable<dynamic> GetBrgyOperator_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_BrgyOperator_List(e));
        }
        public static IDictionary<string, object> Get_BrgyOperator_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["PL_ID"].Str();
            o.PGRPID = data["PGRP_ID"].Str();
            o.AcctType = data["ACT_TYP"].Str();
            o.RequestID = data["USR_ID"].Str();
            o.Operator = textInfo.ToUpper(textInfo.ToLower(data["FLL_NM"].Str()));
            o.MobileNumber = data["MOB_NO"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetAllAcctSearchList(IEnumerable<dynamic> data, int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllAcctSearch_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                //filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllAcctSearch_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllAcctSearch_List(e));
        }
        public static IDictionary<string, object> Get_AllAcctSearch_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PLID = data["PL_ID"].Str();
            o.PGRPID = data["PGRP_ID"].Str();
            o.AcctType = data["ACT_TYP"].Str();
            o.RequestID = data["USR_ID"].Str();
            o.AccountName = textInfo.ToUpper(textInfo.ToLower(data["FLL_NM"].Str()));
            o.MobileNumber = data["MOB_NO"].Str();
            o.ProfileImageUrl = data["IMG_URL"].Str();
            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }


        public static IEnumerable<dynamic> GetAllChatSenderList(IEnumerable<dynamic> data, string userid = "", int limit = 100, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllChatSender_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllChatSender_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllChatSender_List(e));
        }
        public static IDictionary<string, object> Get_AllChatSender_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ChatID = data["ID"].Str();
            o.ChatKey = data["CHT_CD"].Str();
            o.IsPersonal = data["S_PRSNL"].Str();
            o.IsGroup = data["S_GRP"].Str();
            o.IsPublic = data["S_PBLC"].Str();
            o.IsAllowInvatiation = data["S_ALLW_INVT"].Str();
            o.RequestID = data["MMBR_ID"].Str();
            o.DisplayName =  data["FLL_NM"].Str();
            o.ProfileImageUrl = data["IMG_URL"].Str();
            o.MobileNo = data["MOB_NO"].Str();
            o.DateSend = data["RGS_TRN_TS"].Str();
            o.Message = data["MSG"].Str();
            o.IsYou = Convert.ToBoolean(data["IsYou"]);
            //o.IsConnected = (byte)data["IS_CNCTD"] == 0 ? false : true;
            o.IsConnected = (Convert.ToInt32(data["IS_CNCTD"]) == 0) ? false : true;
            o.Count = (data["Unread"].Str() == "0") ? "" : data["Unread"].Str();

            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }

        public static IEnumerable<dynamic> GetAllBlockedAccountList(IEnumerable<dynamic> data, string userid = "", int limit = 30, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllBlockedAccount_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllBlockedAccount_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllBlockedAccount_List(e));
        }
        public static IDictionary<string, object> Get_AllBlockedAccount_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.ChatID = data["CHT_ID"].Str();
            o.ChatKey = data["CHT_CD"].Str();
            o.RequestID = data["USR_ID"].Str();
            o.DisplayName = data["FLL_NM"].Str();
            o.ProfileImageUrl = data["IMG_URL"].Str();
            o.MobileNo = data["MOB_NO"].Str();
            o.IsConnected = Convert.ToBoolean(data["IS_CNCTD"]);

            //o.isLeader = Convert.ToBoolean(data["isLeader"].Str());
            return o;
        }


        public static IEnumerable<dynamic> GetPurposeList(IEnumerable<dynamic> data, int limit = 1000, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetPurpose_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetPurpose_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_Purpose_List(e));
        }
        public static IDictionary<string, object> Get_Purpose_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["PL_ID"].Str();
            o.PGRPID = data["PGRP_ID"].Str();
            o.PurposeID = data["PURP_ID"].Str();
            o.PurposeNM = data["Purpose"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetTemplateTypeList(IEnumerable<dynamic> data, int limit = 1000, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetTemplateType_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetTemplateType_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_TemplateType_List(e));
        }
        public static IDictionary<string, object> Get_TemplateType_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["PL_ID"].Str();
            o.PGRPID = data["PGRP_ID"].Str();
            o.TemplateTypeID = data["TPLTYP_ID"].Str();
            o.TemplateTypeName = data["TPLTTYP_NM"].Str();
            return o;
        }


        public static IEnumerable<dynamic> GetTemplateDocList(IEnumerable<dynamic> data, int limit = 1000, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetTemplateDoc_List(data);
            var count = items.Count();
            //if (count >= limit)
            //{
            //    var o = items.Last();
            //    var filter = (o.NextFilter = Dynamic.Object);
            //    items = items.Take(count - 1).Concat(new[] { o });
            //    filter.NextFilter = o.num_row;
            //}
            return items;
        }
        public static IEnumerable<dynamic> GetTemplateDoc_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_TemplateDoc_List(e));
        }
        public static IDictionary<string, object> Get_TemplateDoc_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PLID = data["PL_ID"].Str();
            o.PGRPID = data["PGRP_ID"].Str();
            o.TemplateTypeID = data["TPLTYP_ID"].Str();
            o.TemplateID = data["TPL_ID"].Str();
            o.TemplateName = data["TPL_NM"].Str();
            return o;
        }

        public static STLAccount STLUpdateMember(STLMembership data)
        {
            STLAccount o = new STLAccount();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.PL_ID = data.PL_ID;
            o.PGRP_ID = data.PGRP_ID;
            o.USR_ID = data.Userid;
            o.ACT_TYP = data.AccountType;

            o.FRST_NM = textInfo.ToTitleCase(data.Firstname);
            o.LST_NM = textInfo.ToTitleCase(data.Lastname);
            o.MDL_NM = textInfo.ToTitleCase(data.Middlename);
            o.FLL_NM = textInfo.ToTitleCase($"{data.Lastname}, {data.Firstname} {data.Middlename}");
            o.NCK_NM = textInfo.ToTitleCase(data.Nickname);

            o.MOB_NO = data.MobileNumber;
            o.EML_ADD = data.EmailAddress;
            o.HM_ADDR = textInfo.ToTitleCase(data.HomeAddress);
            o.PRSNT_ADDR = textInfo.ToTitleCase(data.PresentAddress);
            o.LOC_REG = data.Region;
            o.LOC_PROV = data.Province;
            o.LOC_MUN = data.Municipality;
            o.LOC_BRGY = data.Barangay;
            o.LOC_SIT = data.Sitio;
            o.LOC_SIT_NM = data.SitioName;

            o.GNDR = data.Gender;
            o.MRTL_STAT = data.MaritalStatus;
            o.CTZNSHP = data.Citizenship;
            o.ImageUrl = data.ImageUrl;
            o.BRT_DT = data.BirthDate;
            o.BLD_TYP = data.BloodType;
            o.NATNLTY = textInfo.ToTitleCase(data.Nationality);
            o.OCCPTN = textInfo.ToTitleCase(data.Occupation);
            o.SKLLS = textInfo.ToTitleCase(data.Skills);
            return o;
        }

        public static IEnumerable<dynamic> ConnectionRequestParser(IEnumerable<dynamic> data, string userid = "")
        {
            if (data == null) return null;
            var items = ConnectionRequestObjectParser(data);
            return items;

        }
        public static IEnumerable<dynamic> ConnectionRequestObjectParser(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => ConnectionRequestObject(e));
        }
        public static IDictionary<string, object> ConnectionRequestObject(IDictionary<string, object> data, bool fullinfo = true)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            o.ConnectionRequestId = data["REQ_ID"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.REQ_TO = data["REQ_TO"].Str();
            o.REQ_BY = data["REQ_BY"].Str();
            o.Agenda = data["AGENDA"].Str();
            o.REQ_DTTM = (DateTime)data["REQ_DTTM"];
            o.IsAccepted = (byte)data["IS_ACPTD"] == 0 ? false : true;
            o.IsDeclined = (byte)data["IS_DCLND"] == 0 ? false : true;
            o.IsCanceled = (byte)data["IS_CNCLD"] == 0 ? false : true;
            o.IsConnected = (byte)data["IS_CNCTD"] == 0 ? false : true;
            o.USR_NM = data["USR_NM"].Str();
            o.PRF_PIC = data["PRF_PIC"].Str();
            o.MOB_NO = data["MOB_NO"].Str();
            return o;
        }

        public static IEnumerable<dynamic> GetCommentPostCommunityView(IEnumerable<dynamic> data, string userid = "", int limit = 30, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => GetCommentPostCommunity_View(e));
        }

        public static IDictionary<string, object> GetCommentPostCommunity_View(IDictionary<string, object> e, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.CommunityID = e["COMM_ID"].Str();
            o.PostID = e["POST_ID"].Str();
            o.CommentID = e["CMN_ID"].Str();
            o.CommunityCreated = e["CommunityCreated"].Str();
            o.CommunityImage = e["CommunityImage"].Str();
            o.Post_Title = e["POST_TTL"].Str();
            o.Post_Description = e["POST_DESC"].Str();
            o.Post_ImgContent = e["IMG_CONTENT"].Str();
            o.PosterName = e["PosterName"].Str();
            o.PosterImage = e["PoseterImage"].Str();
            o.isLikePost = Convert.ToInt32(e["isLikePost"]);
            o.isDisLikePost = Convert.ToInt32(e["isDisLikePost"]);
            o.Comment_Count = e["COMMENT_COUNT"].Str();
            o.Community_Total_Like = e["Total_Like"].Str();
            o.Community_Total_DisLike = e["Total_disLike"].Str();
            o.CommunityName = e["COMM_NM"].Str();
            o.CommunityDescription = e["COMM_DESC"].Str();
            o.CommentDescription = e["CMN_DESCRIPTION"].Str();
            o.CommunityLevel = e["COMN_LVL"].Str();
            o.PostDate = e["POST_DT"].Str();
            o.PostTime = Get_Time(Convert.ToDateTime(e["POST_DT"]));
            o.PostTotal_Like = e["Total_Like"].Str();
            o.PostTotal_DisLike = e["Total_disLike"].Str();
            o.CommenterImage = e["CommenterImage"].Str();
            o.CommentDate = e["COMN_DT"].Str();
            o.CommentTime = Get_Time(Convert.ToDateTime(e["COMN_DT"]));
            o.CommunityDate = e["COMM_DT"].Str();
            o.CommunityTime = Get_Time(Convert.ToDateTime(e["COMM_DT"]));
            o.CommenterID = e["CommenterID"].Str();
            o.CommenterName = e["CommenterName"].Str();
            o.Comment_Total_Like = e["TotalComment_Like"].Str();
            o.Comment_Total_DisLike = e["TotalComment_dislike"].Str();
            o.isLikeComment = Convert.ToInt32(e["isLikeComment"]);
            o.isDisLikeComment = Convert.ToInt32(e["isDisLikeComment"]);
            return o;
        }

        public static IEnumerable<dynamic> GetAllCommentPostCommunitiesList(IEnumerable<dynamic> data, string userid = "", int limit = 30, bool fullinfo = true)
        {
            if (data == null) return null;
            var items = GetAllCommentPostCommunities_List(data);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.NextFilter = o.num_row;
                filter.Userid = userid;
            }
            return items;
        }
        public static IEnumerable<dynamic> GetAllCommentPostCommunities_List(IEnumerable<dynamic> data, bool fullinfo = true)
        {
            if (data == null) return null;
            return data.Select(e => Get_AllCommentPostCommunities_List(e));
        }
        public static IDictionary<string, object> Get_AllCommentPostCommunities_List(IDictionary<string, object> data, bool fullinfo = true)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.num_row = data["Num_Row"].Str();
            o.CommunityID = data["COMM_ID"].Str();
            o.CommunityName = data["COMM_NM"].Str();
            o.CommunityDescription = data["COMM_DESC"].Str();
            o.CommunityPostCount = data["POST_COUNT"].Str();
            o.CommunityTypeLevel = data["TYP_LVL"].Str();
            o.CommunityTypeLevelDescription = data["Typ_Level_Desc"].Str();
            return o;
        }

        public static string Get_Time(DateTime Post_Date)
        {
            var milliseconds = DateTime.Now.Subtract(Post_Date);
            var d = milliseconds.Days;
            var h = milliseconds.Hours;
            var mi = milliseconds.Minutes;
            var ss = milliseconds.Seconds;
            var mls = milliseconds.Milliseconds;
            if (d == 0 && h == 0 && mi == 0 && ss > 0)
            {
                if (ss == 1)
                    return ss + " second ago";
                return ss + " seconds ago";
            }
            else if (d == 0 && h == 0 && mi < 60)
            {
                if (mi == 1)
                    return mi + " minute ago";
                return mi + " minutes ago";
            }
            else if (d == 0 && h < 24)
            {
                if (h == 1)
                    return h + " hour ago";
                return h + " hours ago";
            }
            else if (d >= 1) //days
            {
                if (d == 1)
                    return d + " day ago";
                else if (d > 1 && d < 365)
                {
                    if (d < 30)
                        return d + " days ago";
                    var dd = d / 30;
                    if (dd == 1)
                    {
                        return "1 month ago";
                    }
                    else if (dd > 1 && dd <= 12)
                    {
                        return dd + " months ago";
                    }
                }
                else
                {
                    var dd = d / 365;
                    if (dd == 1)
                        return "1 year ago";
                    else
                        return dd + " years ago";
                }
            }
            return "1 second ago";
        }
    }
}
