using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Model.User;
using webapi.App.Aggregates.Common.Dto;
using System.Globalization;

namespace webapi.App.Aggregates.STLPartylistMembership
{
    [Service.ITransient(typeof(AccountRepository))]
    public interface IAccountRepository
    {
        Task<(SignInResults result, String message, String apkVersion, String apkUrl)> ApkUpdateCheckerAsync(STLSignInRequest request);
        Task<(SignInResults result, String message, STLAccount account)> STLSignInAsync(STLSignInRequest request);
        Task<(Results result, String message)> RequiredChangePassword(RequiredChangePassword request);
        Task<(object PartyList, object Group)> MemberGroup(STLAccount account);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly IRepository _repo;
        public AccountRepository(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<(SignInResults result, string message, string apkVersion, string apkUrl)> ApkUpdateCheckerAsync(STLSignInRequest request)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_CBA0A", new Dictionary<string, object>(){
                {"parmplid",request.plid },
                {"parmpgrpid",request.groupid },
                {"parmpsncd",request.psncd },
                { "parmapkversion", request.ApkVersion },
            });
            if (results != null)
            {
                var result = ((IDictionary<string, object>)results.ReadSingleOrDefault());
                if (result == null)
                    return (SignInResults.Success, null, null, null);
                return (SignInResults.ApkUpdate, "New version available", result["APP_VRSN"].Str(), result["APK_UPDT_URL"].Str());
            }
            return (SignInResults.Null, null, null, null);
        }

        public async Task<(object PartyList, object Group)> MemberGroup(STLAccount account)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_CBA01", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                //{"parmpsncd",account.PSN_CD }
            });
            if (results != null)
            {
                var Group = STLSubscriberDto.GetGroup(results.ReadSingleOrDefault());
                var PartyList = STLSubscriberDto.GetPartyList(results.ReadSingleOrDefault());
                return (PartyList, Group);
            }
            return (null, null);


        }

        public async Task<(Results result, string message)> RequiredChangePassword(RequiredChangePassword request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDAS0FCP", new Dictionary<string, object>()
            {
                {"parmplid", request.PLID },
                {"parmpgrpid", request.PGRPID },
                {"parmusrename", request.Username },
                {"parmpassword", request.OldPassword },
                {"parmnewpassword", request.Password },
                {"parmconfirmpassword", request.ConfirmPassword }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Change Successfull! you can now use your new password");
                else if (ResultCode == "61")
                    return (Results.Null, "Password did not match");
                else if (ResultCode == "62")
                    return (Results.Null, "You are trying to user your old password, please try again.");
                else if (ResultCode == "0")
                    return (Results.Null, "Your username or mobile number was not exist, please try again.");
                else if (ResultCode == "21")
                    return (Results.Null, "You are try to access block account, please try again.");
                return (Results.Null, "Failed to Change! your request is already done");
            }
            return (Results.Null, null);
        }

        public async Task<(SignInResults result, String message, STLAccount account)> STLSignInAsync(STLSignInRequest request)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            var results = _repo.DSpQueryMultiple("dbo.spfn_AABOL", new Dictionary<string, object>()
            {
                {"parmusrename",request.Username },
                {"parmpassword",request.Password },
                //{"parmplid",request.plid },
                //{"parmpgrpid",request.groupid },
                //{"parmpsncd",request.psncd }
            });
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results.ReadFirstOrDefault());
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (SignInResults.Success, null, new STLAccount()
                    {
                        PL_ID = row["PL_ID"].Str(),
                        PGRP_ID = row["PGRP_ID"].Str(),
                        PSN_CD = row["PSN_CD"].Str(),
                        Username = row["USR_NM"].Str(),
                        isMember = Convert.ToBoolean(row["isMember"].Str()),
                        isLeader = Convert.ToBoolean(row["isLeader"].Str()),
                        isGroup = Convert.ToBoolean(row["isGroup"].Str()),
                        isUser = Convert.ToBoolean(row["isUser"].Str()),
                        REF_GRP_ID = row["REF_GRP_ID"].Str(),
                        REF_LDR_ID = row["REF_LDR_ID"].Str(),
                        LDR_NM = textInfo.ToTitleCase(textInfo.ToLower(row["LDR_NM"].Str())),
                        USR_ID = row["USR_ID"].Str(),
                        ACT_ID = row["ACT_ID"].Str(),
                        ACT_TYP = row["ACT_TYP"].Str(),
                        PRCNT_NO = row["PRCNT_NO"].Str(),
                        CLSTR_NO = row["CLSTR_NO"].Str(),
                        FRST_NM = textInfo.ToTitleCase(row["FRST_NM"].Str()),
                        LST_NM = textInfo.ToTitleCase(row["LST_NM"].Str()),
                        MDL_NM = textInfo.ToTitleCase(row["MDL_NM"].Str()),
                        FLL_NM = textInfo.ToTitleCase(row["FLL_NM"].Str()),
                        NCK_NM = textInfo.ToTitleCase(row["NCK_NM"].Str()),

                        MOB_NO = row["MOB_NO"].Str(),
                        EML_ADD = row["EML_ADD"].Str(),
                        HM_ADDR = textInfo.ToUpper(row["HM_ADDR"].Str()),
                        PRSNT_ADDR = textInfo.ToUpper(row["PRSNT_ADDR"].Str()),
                        LOC_REG = row["LOC_REG"].Str(),
                        LOC_PROV = row["LOC_PROV"].Str(),
                        LOC_MUN = row["LOC_MUN"].Str(),
                        LOC_BRGY = row["LOC_BRGY"].Str(),
                        LOC_BRGY_NM = textInfo.ToUpper(row["LOC_BRGY_NM"].Str()),
                        LOC_SIT = row["LOC_SIT"].Str(),
                        LOC_SIT_NM = textInfo.ToUpper(row["LOC_SIT_NM"].Str()),

                        GNDR = row["GNDR"].Str(),
                        MRTL_STAT = row["MRTL_STAT"].Str(),
                        CTZNSHP = row["CTZNSHP"].Str(),
                        ImageUrl = row["IMG_URL"].Str(),
                        BRT_DT = row["BRT_DT"].Str(),
                        BLD_TYP = row["BLD_TYP"].Str(),
                        NATNLTY = textInfo.ToTitleCase(row["NATNLTY"].Str()),
                        OCCPTN = textInfo.ToTitleCase(row["OCCPTN"].Str()),
                        SKLLS = textInfo.ToTitleCase(row["SKLLS"].Str()),
                        PRF_PIC = row["PRF_PIC"].Str(),
                        SIGNATUREID = row["SIGNATUREID"].Str(),
                        S_ACTV = row["S_ACTV"].Str(),
                        SessionID = row["SSSN_ID"].Str(),
                        TTL_BIZ = Convert.ToInt32(row["TTL_BIZ"].Str()),
                        SUB_TYP = row["SUB_TYP"].Str(),
                        sActive = true,
                        IsLogin = true
                    }); 
                }
                else if (ResultCode == "52")
                    return (SignInResults.ChangePassword, "Your are required to change password", null);
                else if (ResultCode == "21")
                    return (SignInResults.Failed, "Your account has blocked by admin", null);
                else if (ResultCode == "22")
                    return (SignInResults.Failed, "Party List has blocked by admin", null);
                else if (ResultCode == "23")
                    return (SignInResults.Failed, "Your Group has blocked by admin", null);
                return (SignInResults.Failed, "Invalid mobile number and password! Please try again", null);
            }
            return (SignInResults.Null, null, null);
        }

    }
}
