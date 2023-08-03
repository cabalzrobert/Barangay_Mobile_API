using System;
using System.Collections.Generic;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using System.Linq;
using System.Globalization;

namespace webapi.App.Aggregates.Common
{
    public class SubscriberDto
    {
        public static IDictionary<string, object> AccountBalance(STLAccount account, IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            o.Balance = data["ACT_BAL"].Str().ToDecimalDouble();
            o.CreditBalance = data["ACT_CRDT_BAL"].Str().ToDecimalDouble();
            //if(!account.IsPlayer){
            //    var combal = data["ACT_COM_BAL"].Str().ToDecimalDouble();
            //    o.CommissionBalance = combal;
            //}
            var winbal = data["ACT_WIN_BAL"].Str().ToDecimalDouble();
            if(winbal>0) o.WinningBalance = winbal;
            return o;
        }

        public static IDictionary<string, object> EmergencyAlertNotification(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.UserID = data["USR_ID"].Str();
            o.AccountName = data["FLL_NM"].Str();
            o.ProfileImageUrl = data["IMG_URL"].Str();
            o.MobileNo = data["MOB_NO"].Str();
            o.EmergencyID = data["EMGY_ID"].Str();
            o.EmergencyTypeID = data["EMGY_TYP_ID"].Str();
            o.EmergencyTypeNM = data["EMERGENCY_TYPE"].Str();
            o.GeoLocationLat = data["GEO_LOC_LAT"].Str();
            o.GeoLocationLong = data["GEO_LOC_LONG"].Str();
            o.DateReceived = Convert.ToDateTime(data["RGS_TRN_TS"].Str()).ToString("MMM dd, yyyy");
            o.Count_Request = data["CNT_REQUEST"].Str();
            return o;
        }
        public static IDictionary<string, object> RequestDocumentNotification(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.RequestDocument = data["RequestDocument"].Str();
            o.Doc_ID = data["DOC_ID"].Str();
            o.ClearanceNo = data["ID"].Str();
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.UserID = data["USR_ID"].Str();
            o.ResidentIDNo = data["RESIDENT_ID_NO"].Str();
            o.Requestor = textInfo.ToUpper(textInfo.ToLower(data["FLL_NM"].Str()));
            o.ProfileImageUrl = data["IMG_URL"].Str();
            o.MobileNo = data["MOB_NO"].Str();
            o.ControlNo = data["CNTRL_NO"].Str();
            o.ClearanceType = data["CERTTYP_ID"].Str();
            o.Requested_Date = data["Requested_Date"].Str();

            return o;
        }
        public static IDictionary<string, object> RequestBarangayClearanceNotification1(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            o.RequestDocument = "Barangay Clearance";
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.UserID = data["USR_ID"].Str();
            o.MobileNo = data["MOB_NO"].Str();
            o.ProfileImageUrl = data["IMG_URL"].Str();
            o.ClearanceNo = data["BRGYCLR_ID"].Str();
            o.ControlNo = data["CNTRL_NO"].Str();
            o.ResidentIDNo = data["RESIDENT_ID_NO"].Str();
            o.Requestor = textInfo.ToUpper(textInfo.ToLower(data["FLL_NM"].Str()));
            o.TypeofClearance = data["CERTTYP_ID"].Str();
            o.ClearanceType = textInfo.ToUpper(textInfo.ToLower(data["CERTTYP_NM"].Str()));
            o.Purpose = data["PURP_ID"].Str();
            o.PurposeNM = textInfo.ToUpper(textInfo.ToLower(data["PURP_NM"].Str()));

            o.ORNumber = textInfo.ToUpper(textInfo.ToLower(data["OR_NO"].Str()));
            //o.AmountPaid = Convert.ToDecimal(string.Format("{0:#.0}", data["AMOUNT_PAID"].Str()));
            o.AmountPaid = (data["AMOUNT_PAID"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDouble(data["AMOUNT_PAID"].Str()).ToString("n2");
            o.DocStamp = (data["DOC_STAMP"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDecimal(data["DOC_STAMP"].Str()).ToString("n2");
            o.TotalAmount = (data["TTL_AMNT"].Str() == "") ? Convert.ToDecimal(0).ToString("n2") : Convert.ToDecimal(data["TTL_AMNT"].Str()).ToString("n2");

            o.EnableCommunityTax = Convert.ToBoolean(data["ENABLECTC"].Str());
            o.CTCNo = textInfo.ToUpper(textInfo.ToLower(data["CTC"].Str()));
            o.CTCIssuedAt = textInfo.ToUpper(textInfo.ToLower(data["CTCISSUEDAT"].Str()));
            o.CTCIssuedOn = (data["CTCISSUEDON"].Str() == "") ? "" : Convert.ToDateTime(data["CTCISSUEDON"].Str()).ToString("MMM dd, yyyy");

            o.VerifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["VERIFIEDBY"].Str()));
            o.CertifiedBy = textInfo.ToTitleCase(textInfo.ToLower(data["CERTIFIEDBY"].Str()));

            o.Release = (data["RELEASED"].Str() == "") ? false : Convert.ToBoolean(data["RELEASED"].Str());
            o.MosValidity = (data["MOS_VAL"].Str() == "") ? 0 : Convert.ToInt32(data["MOS_VAL"].Str());
            o.IssuedDate = (data["DATEPROCESS"].Str() == "") ? "" : Convert.ToDateTime(data["DATEPROCESS"].Str()).ToString("MMM dd, yyyy");
            o.DateRelease = (data["DATERELEASED"].Str() == "") ? "" : Convert.ToDateTime(data["DATERELEASED"].Str()).ToString("MMM dd, yyyy");
            o.ExpiryDate = (data["VALIDITYDATE"].Str() == "") ? "" : Convert.ToDateTime(data["VALIDITYDATE"].Str()).ToString("MMM dd, yyyy");
            o.ReleasedBy = textInfo.ToUpper(textInfo.ToLower(data["RELEASEDBY"].Str()));
            o.Cancelled = (data["CANCELLED"].Str() == "") ? false : Convert.ToBoolean(data["CANCELLED"].Str());
            o.CancelledBy = textInfo.ToUpper(textInfo.ToLower(data["CANCELLEDBY"].Str()));
            o.CancelledDate = (data["DATECANCELLED"].Str() == "") ? "" : Convert.ToDateTime(data["DATECANCELLED"].Str()).ToString("MMM dd, yyyy");
            o.AppointmentDate = (data["APP_DATE"].Str() == "") ? "" : Convert.ToDateTime(data["APP_DATE"].Str()).ToString("MMM dd, yyyy");

            return o;
        }



        public static IDictionary<string, object> ComplaintNotification(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            //10
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.CMPLNT_ID = data["CMPLNT_ID"].Str();
            o.CMPLNTS = data["CMPLNTS"].Str();
            o.MobileNo = data["MOB_NO"].Str();
            o.ProfileImageUrl = data["IMG_URL"].Str();
            o.STS_TYP = data["STS_TYP"].Str();
            o.STATUS = data["STATUS"].Str();
            o.BRGY_CASE_NO = data["BRGY_CASE_NO"].Str();
            o.CMPLNT_SIT = data["CMPLNT_SIT"].Str();

            //20
            o.RSPNDT_ID = data["RSPNDT_ID"].Str();
            o.RSPNDTS = data["RSPNDTS"].Str();
            o.RSPNDT_SIT = data["RSPNDT_SIT"].Str();
            o.WTNS = data["WTNS"].Str();
            o.ACCUSATION = data["ACCUSATION"].Str();
            o.CMPLNT_TYP = data["CMPLNT_TYP"].Str();
            o.CMPLNT_TYP_NM = data["CMPLNT_TYP_NM"].Str();
            o.INCIDENT_PLACE = data["INCIDENT_PLACE"].Str();
            o.INCIDENT_DT = data["INCIDENT_DT"].Str();
            o.INCIDENT_TIME = data["INCIDENT_TIME"].Str();

            //30
            o.STATEMENT = data["STATEMENT"].Str();
            o.CREATED_DT = data["CREATED_DT"].Str();
            o.MDTN_DT = data["MDTN_DT"].Str();
            o.MDTN_TM = data["MDTN_TM"].Str();
            o.CLTN_DT = data["CLTN_DT"].Str();
            o.CLTN_TM = data["CLTN_TM"].Str();
            o.ABTN_DT = data["ABTN_DT"].Str();
            o.ABTN_TM = data["ABTN_TM"].Str();
            o.STLMT = data["STLMT"].Str();
            o.STLMT_AWRD = data["STLMT_AWRD"].Str();

            //40
            o.STLMT_DT = data["STLMT_DT"].Str();
            o.STLMT_TM = data["STLMT_TM"].Str();
            o.FTA_DT = data["FTA_DT"].Str();
            o.FTA_TM = data["FTA_TM"].Str();
            o.FL_DT = data["FL_DT"].Str();
            o.FL_TM = data["FL_TM"].Str();
            o.ABTN_AWRD = data["ABTN_AWRD"].Str();
            o.ABTN_AWRD_DT = data["ABTN_AWRD_DT"].Str();
            o.PKT_CHRMN = data["PKT_CHRMN"].Str();
            o.PKT_SCRTY = data["PKT_SCRTY"].Str();

            //45
            o.PKT_FMBR = data["PKT_FMBR"].Str();
            o.PKT_SMBR = data["PKT_SMBR"].Str();
            o.ABTN_AWRD_TM = data["ABTN_AWRD_TM"].Str();
            o.FTS = data["FTS"].Str();
            o.STL_DT = data["STL_DT"].Str();
            //48
            o.CFA_DT = data["CFA_DT"].Str();
            o.CFA_TM = data["CFA_TM"].Str();
            o.MODIFIED_DT = data["MODIFIED_DT"].Str();

            return o;
        }

        public static IDictionary<string, object> IssuesandConcerntNotification(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            //3
            o.PL_ID = data["PL_ID"].Str();
            o.PGRP_ID = data["PGRP_ID"].Str();
            o.Userid = data["USR_ID"].Str();

            //10
            o.Firstname = textInfo.ToTitleCase(data["FRST_NM"].Str());
            o.Lastname = textInfo.ToTitleCase(data["LST_NM"].Str());
            o.Middlename = textInfo.ToTitleCase(data["MDL_NM"].Str());
            o.AccountName = textInfo.ToTitleCase(data["FLL_NM"].Str());
            o.MobileNumber = data["MOB_NO"].Str();
            o.ProfileImageUrl = data["IMG_URL"].Str();
            o.Sitio = data["LOC_SIT"].Str();
            o.SitioName = textInfo.ToTitleCase(data["SIT_NM"].Str());
            o.GeoLocationLat = data["GEO_LOC_LAT"].Str();
            o.GeoLocationLong = data["GEO_LOC_LONG"].Str();
            //10
            o.TransactionNo = data["TRN_NO"].Str();
            o.TicketNo = data["TCKT_NO"].Str();
            o.Subject = data["SBJCT"].Str();
            o.Body = data["BODY"].Str();
            o.Status = data["STAT"].Str();
            o.StatusName = data["STAT_NM"].Str();
            o.CorrectiveAction = data["COR_ACTION"].Str();
            o.DateReceived = (data["RGS_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["RGS_TRN_TS"].Str()).ToString("MMM, dd, yyyy");
            o.ProcessDate = (data["PRCS_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["PRCS_TRN_TS"].Str()).ToString("MMM dd, yyyy");
            o.ActionDate = (data["FXD_TRN_TS"].Str() == "") ? "" : Convert.ToDateTime(data["FXD_TRN_TS"].Str()).ToString("MMM, dd, yyyy");
            //2
            o.TotalAttachment = (data["TTL_ATTCHMNT"].Str() == "0") ? "" : data["TTL_ATTCHMNT"].Str();
            o.Attachment = (data["ATTCHMNT"].Str() == "0") ? "" : data["ATTCHMNT"].Str();
            return o;
        }

        public static IEnumerable<dynamic> EventNotification(STLAccount account, IEnumerable<dynamic> list, int limit=50)
        {
            if (list == null) return null;
            var items = SearchEvents(account, list);
            var count = items.Count();
            if (count >= limit)
            {
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count - 1).Concat(new[] { o });
                filter.Basefilter = o.EventID;
            }

            return items;
        }

        public static IEnumerable<dynamic> SearchEvents(STLAccount account, IEnumerable<dynamic> list)
        {
            if (list == null) return null;
            return list.Select(e => SearchEvent(account, e));
        }

        public static IDictionary<string, object> SearchEvent(STLAccount account, IDictionary<string, object> data)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            dynamic o = Dynamic.Object;
            //o.EventID = ((int)data["EVENT_ID"].Str().ToDecimalDouble()).ToString("X");
            o.EventID = data["EVENT_ID"].Str();
            o.DateTransaction = data["RGS_TRN_TS"].Str();
            o.Title = textInfo.ToTitleCase(textInfo.ToLower(data["EVENT_TTL"].Str()));
            o.Description = textInfo.ToTitleCase(textInfo.ToLower(data["EVENT_DESC"].Str()));
            o.EventDateTime = $"{Convert.ToDateTime(data["EVENT_DATE"]).ToString("MMM dd, yyyy").Str()} {data["EVENT_TIME"].Str()}";
            o.EventDate = Convert.ToDateTime(data["EVENT_DATE"]).ToString("MMM dd, yyyy").Str();
            o.EventTime = data["EVENT_TIME"].Str();
            o.Location = textInfo.ToTitleCase(textInfo.ToLower(data["EVENT_LOCATION"].Str()));
            o.ProccessID = data["PRCSS_ID"].Str();
            return o;
        }

        public static IDictionary<string, object> EventNofitication(IDictionary<string, object> data)
        {
            dynamic o = Dynamic.Object;
            o.NotifcationID = ((int)data["NOTIF_ID"].Str().ToDecimalDouble()).ToString("X");
            o.DateTransaction = data["RGS_TRN_TS"];
            o.Title = data["NOTIF_TTL"].Str();
            o.Description = data["NOTIF_DESC"].Str();
            o.IsCompany = data["S_COMP"].To<bool>(false);
            o.IsOpen = data["S_OPN"].To<bool>(false);
            string type = data["TYP"].Str();
            if (!type.IsEmpty()) o.Type = type;

            try
            {
                DateTime datetime = data["RGS_TRN_TS"].To<DateTime>();
                o.DateDisplay = datetime.ToString("MMM dd, yyyy");
                o.TimeDisplay = datetime.ToString("hh:mm:ss tt");
                o.FulldateDisplay = $"{o.DateDisplay} {o.TimeDisplay}";
            }
            catch { }
            return o;
        }

        public static IEnumerable<dynamic> SearchSubscribers(STLAccount account, IEnumerable<dynamic> list, int limit = 50){
            if(list==null) return null;
            var items = SearchSubscribers(account, list);
            var count = items.Count();
            if(count>=limit){
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count-1).Concat(new[]{ o });
                filter.BaseFilter = o.AccountID;
            }
            return items;
        }

        public static IEnumerable<dynamic> SearchSubscribers(STLAccount account, IEnumerable<dynamic> list){
            if(list==null) return null;
            return list.Select(e=> SearchSubscriber(account, e));
        }

        public static IDictionary<string, object> SearchSubscriber(STLAccount account,  IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            //o.SubscriberID =  data["CUST_ID"].Str();
            o.AccountID =  data["ACT_ID"].Str();
            o.MobileNumber = data["MOB_NO"].Str();
            o.DisplayName =  data["NCK_NM"].Str();
            o.ImageUrl =  data["IMG_URL"].Str();
            o.Firstname = data["FRST_NM"].Str().ToUpper();
            o.Lastname = data["LST_NM"].Str().ToUpper();
            o.Fullname = data["FLL_NM"].Str().ToUpper();
            o.CreditBalance = data["ACT_CRDT_BAL"].Str().ToDecimalDouble();
            o.CommissionBalance = data["ACT_COM_BAL"].Str().ToDecimalDouble();
            //
            string usertype = data["USR_TYP"].Str();
            o.IsPlayer = (usertype.Equals("5"));
            //if(account.IsGeneralCoordinator){
            //    o.IsCoordinator = (usertype.Equals("4"));
            //    o.IsGeneralCoordinator = (usertype.Equals("3"));
            //}
            o.IsBlocked = (data["S_BLCK"].Str().Equals("1"));
            return o;
        }


        public static IEnumerable<dynamic> SearchSubscribers(IEnumerable<dynamic> list, int limit = 50){
            if(list==null) return null;
            var items = SearchSubscribers(list);
            var count = items.Count();
            if(count>=limit){
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count-1).Concat(new[]{ o });
                filter.BaseFilter = o.AccountID;
            }
            return items;
        }

        public static IEnumerable<dynamic> SearchSubscribers(IEnumerable<dynamic> list){
            if(list==null) return null;
            return list.Select(e=> SearchSubscriber(e));
        }

        public static IDictionary<string, object> SearchSubscriber(IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            //o.SubscriberID =  data["CUST_ID"].Str();
            o.AccountID =  data["ACT_ID"].Str();
            o.MobileNumber = data["MOB_NO"].Str().Replace("+639","09");
            //o.DisplayName =  data["NCK_NM"].Str();
            o.Fullname = data["FLL_NM"].Str().ToUpper();
            o.ImageUrl =  data["IMG_URL"].Str();
            /*o.Firstname = data["FRST_NM"].Str().ToUpper();
            o.Lastname = data["LST_NM"].Str().ToUpper();
            o.Fullname = data["FLL_NM"].Str().ToUpper();
            o.CreditBalance = data["ACT_CRDT_BAL"].Str().ToDecimalDouble();
            o.CommissionBalance = data["ACT_COM_BAL"].Str().ToDecimalDouble();
            //
            string usertype = data["USR_TYP"].Str();
            o.IsPlayer = (usertype.Equals("5"));
            if(account.IsGeneralCoordinator){
                o.IsCoordinator = (usertype.Equals("4"));
                o.IsGeneralCoordinator = (usertype.Equals("3"));
            }
            o.IsPlayer = (usertype.Equals("5"));
            o.IsCoordinator = (usertype.Equals("4"));
            o.IsGeneralCoordinator = (usertype.Equals("3"));
            o.IsBlocked = (data["S_BLCK"].Str().Equals("1"));*/
            return o;
        }


        
        public static IEnumerable<dynamic> SearchRegisters(IEnumerable<dynamic> list, int limit = 50){
            if(list==null) return null;
            var items = SearchRegisters(list);
            var count = items.Count();
            if(count>=limit){
                var o = items.Last();
                var filter = (o.NextFilter = Dynamic.Object);
                items = items.Take(count-1).Concat(new[]{ o });
                filter.BaseFilter = o.RegisterID;
            }
            return items;
        }

        public static IEnumerable<dynamic> SearchRegisters(IEnumerable<dynamic> list){
            if(list==null) return null;
            return list.Select(e=> SearchRegister(e));
        }

        public static IDictionary<string, object> SearchRegister(IDictionary<string, object> data){
            dynamic o = Dynamic.Object;
            //o.SubscriberID =  data["CUST_ID"].Str();
            o.RegisterID =  data["RGS_ID"].Str();
            o.MobileNumber = data["MOB_NO"].Str().Replace("+639","09");
            //o.DisplayName =  data["NCK_NM"].Str();
            o.ImageUrl =  data["IMG_URL"].Str();
            //
            o.Type =  (int)data["USR_TYP"].Str().ToDecimalDouble();
            o.Role =  (int)data["GRP_CD"].Str().ToDecimalDouble();
            //o.RoleName =  data["NCK_NM"].Str();
            //
            o.Fullname =  data["FLL_NM"].Str();
            o.Lastname =  data["LST_NM"].Str();
            o.Middlename =  data["MDL_NM"].Str();
            o.Firstname =  data["FRST_NM"].Str();

            o.BirthDate =  data["BRT_DT"].Str();
            o.Gender =  data["GNDR"].Str();
            o.BloodType =  data["BLD_TYP"].Str();
            o.Nationality =  data["NATNLTY"].Str();
            o.MaritalStatus =  data["MRTL_STAT"].Str();

            //o.MobileNumber =  data["MOB_NO"].Str();
            o.EmailAddress =  data["EML_ADD"].Str();
            o.Address =  data["PRSNT_ADDR"].Str();
            o.Occupation =  data["OCCPTN"].Str();
            o.Skills =  data["SKLLS"].Str();
            o.GeneralCoordinator =  data["REF_ACT_ID"].Str();
            o.Coordinator =  data["REF_ACT_ID2"].Str();
            o.GeneralCoordinatorName =  data["REF_ACT_NM"].Str();
            o.CoordinatorName =  data["REF_ACT_NM2"].Str();

            o.Region =  data["LOC_REG"].Str();
            o.Province =  data["LOC_PROV"].Str();
            o.Municipality =  data["LOC_MUN"].Str();
            o.Barangay =  data["LOC_BRGY"].Str();

            return o;
        }
    }

}