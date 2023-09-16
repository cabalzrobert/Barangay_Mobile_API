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

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(STLMembershipRepository))]
    public interface ISTLMembershipRepository
    {
        ////Task<(Results result, String message, STLSignInRequest signin, STLAccount account)> MembershipAsync(STLMembership membership, bool isUpdate = false);
        Task<(Results result, String message)> MembershipAsync(ResidentsInfo membership, bool isUpdate = false);

        Task<(Results result, String message)> GetBarangaySubscriberAsync(ResidentsInfo membership);
        Task<(Results result, String message)> UpdateMembershipAsync(ResidentsInfo membership);

        Task<(Results result, object reglist)> RegionList(LocationInfo req);
        Task<(Results result, object provlist)> ProvinceList(LocationInfo req);
        Task<(Results result, object munlist)> MunicipalityList(LocationInfo req);
        Task<(Results result, object brgylist)> BarangayList(LocationInfo req);
        //Task<(Results result, object bryoptr)> Load_BrgyOperator(FilterRequest req);
    }
    public class STLMembershipRepository : ISTLMembershipRepository
    {
        //private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        //public STLAccount account { get { return _identity.AccountIdentity(); } }
        public STLMembershipRepository(/*ISubscriber identity,*/ IRepository repo)
        {
            //_identity = identity;
            _repo = repo;
        }



        //public async Task<(Results result, object bryoptr)> Load_BrgyOperator(FilterRequest req)
        //{
        //    var result = _repo.DSpQueryMultiple($"dbo.spfn_BDB0E", new Dictionary<string, object>()
        //    {
        //        {"parmplid",account.PL_ID },
        //        {"parmpgrpid",account.PGRP_ID },
        //        {"parmsearch", req.Search }
        //    });
        //    if (result != null)
        //        return (Results.Success, STLSubscriberDto.GetBrgyOperatorList(result.Read<dynamic>(), 1000));

        //    return (Results.Null, null);
        //}
        //public async Task<(Results result, string message, STLSignInRequest signin, STLAccount account)> MembershipAsync(STLMembership membership, bool isUpdate = false)
        public async Task<(Results result, string message)> MembershipAsync(ResidentsInfo info, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDABDBCAACBB01", new Dictionary<string, object>()
            {
                {"parmplid",info.PL_ID },
                {"parmpgrpid",info.PGRP_ID },
                {"parmfnm",info.Firstname },
                {"parmlnm",info.Lastname },
                {"parmmnm",info.Middlename },
                {"parmnnm",info.Nickname },
                {"parmmobno",info.MobileNumber },
                {"parmgender",info.Gender },
                {"parmmstastus",info.MaritalStatus },
                {"parmemladd",info.EmailAddress },
                {"parmhmeadd",info.HomeAddress },
                {"parmprsntadd",info.PresentAddress},
                {"parmprecentno", info.PrecinctNumber},
                {"parmclusterno", info.ClusterNumber },
                {"parmreg",info.Region },
                {"parmprov",info.Province },
                {"parmmun",info.Municipality },
                {"parmbrgy",info.Barangay },
                {"parmsitio",info.Sitio },
                {"parmbdate",info.BirthDate },
                {"parmctznshp",info.Citizenship },
                {"parmbldType",info.BloodType },
                {"parmoccptn",info.Occupation },
                {"parmsklls",info.Skills },
                {"parmprfpic",info.ImageUrl },
                {"parmImgUrl",info.ImageUrl },
                {"parmusertype",info.AccountType },

                {"parmusername",info.Username },
                //{"parmpassword",membership.Userpassword },
                {"parmusrid",(isUpdate?info.Userid:"") },
                {"parmrlgn",info.Religion },
                {"parmhght",info.Height },
                {"parmwght",info.Weight+"kg" },
                {"parmbrtpl",info.Birthplace },
                //{"parmfrfnm",info?.Father.Firstname ?? null},
                //{"parmfrmnm",info?.Father.Middlename ?? null},
                //{"parmfrlnm",info?.Father.Lastname ?? null},
                //{"parmfrfllnm",info?.Father.Fullname ?? null},
                //{"parmmrfnm",info?.Mother.Firstname ?? null},
                //{"parmmrmnm",info?.Mother.Middlename ?? null},
                //{"parmmrlnm",info?.Mother.Lastname ?? null},
                //{"parmmrfllnm",info?.Mother.Fullname ?? null},
                //{"parmspfllnm",info?.Spouse.Fullname ?? null},
                //{"parmsplb",(info.IsParentLiveInBarangay) ? 1 : 0},
                {"parmslwp",(info.IsLivingWithParents) ? 1 : 0},
                {"parmssrctzn",(info.IsSeniorCitizen) ? 1 : 0},
                {"parmssp",(info.IsSingleParent) ? 1 : 0},
                {"parmsindgt",(info.IsIndigent) ? 1 : 0},
                {"parmspwd",(info.IsPWD) ? 1 : 0},
                {"parmrgstvtr",(info.IsRegisteredVoter) ? 1 : 0},
                {"parmspermrsdnt",1},
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    //if (isUpdate == false)
                    //{
                    //    return (Results.Success, "Membership Successfull Save", new STLSignInRequest()
                    //    {
                    //        Username = row["USR_NM"].Str(),
                    //        Password = row["USR_PSSWRD"].Str(),
                    //        plid = row["PL_ID"].Str(),
                    //        groupid = row["PGRP_ID"].Str(),
                    //        psncd = row["PSN_CD"].Str()
                    //    },null);
                    //}
                    //else
                    //{
                    //return (Results.Success, "Member Account Successful save", null, STLSubscriberDto.STLUpdateMember(membership));
                    return (Results.Success, "submission completed. For verification, kindly wait 24 hours.");
                //}

                else if (ResultCode == "2")
                    //return (Results.Failed, "Invalid Mobile Number", null, null);
                    return (Results.Failed, "Invalid Mobile Number");
                else if (ResultCode == "3")
                    //return (Results.Failed, "Mobile Number already exist", null, null);
                    return (Results.Failed, "Mobile Number already exist");
                else if (ResultCode == "4")
                    //return (Results.Failed, "You are already Member of this Group", null, null);
                    return (Results.Failed, "You are already Member of this Group");
                else if (ResultCode == "5")
                    //return (Results.Failed, "Username already exist", null, null);
                    return (Results.Failed, "Username already exist");
            }
            //return (Results.Null, null, null, null);
            return (Results.Null, null);
        }



        public async Task<(Results result, object reglist)> RegionList(LocationInfo req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSREG", new Dictionary<string, object>()
            {
                {"parmsearch",req.Search }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
        public async Task<(Results result, object provlist)> ProvinceList(LocationInfo req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSPROV", new Dictionary<string, object>()
            {
                {"parmcode", req.ID },
                {"parmsearch",req.Search }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
        public async Task<(Results result, object munlist)> MunicipalityList(LocationInfo req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSMUN", new Dictionary<string, object>()
            {
                {"parmcode", req.ID },
                {"parmsearch",req.Search }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }
        public async Task<(Results result, object brgylist)> BarangayList(LocationInfo req)
        {
            var result = _repo.DSpQuery<dynamic>($"spfn_BIMSBRGY0A", new Dictionary<string, object>()
            {
                {"parmcode", req.ID },
                {"parmsearch",req.Search }
            });
            if (result != null)
                return (Results.Success, result);
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> UpdateMembershipAsync(ResidentsInfo info)
        {
            //bool isleader = Convert.ToBoolean(info.isLeader);
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDABDBCAACBB05", new Dictionary<string, object>()
            {
                {"parmplid",info.PL_ID },
                {"parmpgrpid",info.PGRP_ID },
                {"parmfnm",info.Firstname },
                {"parmlnm",info.Lastname },
                {"parmmnm",info.Middlename },
                {"parmnnm",info.Nickname },
                {"parmmobno",info.MobileNumber },
                {"parmgender",info.Gender },
                {"parmmstastus",info.MaritalStatus },
                {"parmemladd",info.EmailAddress },
                {"parmhmeadd",info.HomeAddress },
                {"parmprsntadd",info.PresentAddress},
                {"parmprecentno", info.PrecinctNumber},
                {"parmclusterno", info.ClusterNumber },
                {"parmreg",info.Region },
                {"parmprov",info.Province },
                {"parmmun",info.Municipality },
                {"parmbrgy",info.Barangay },
                {"parmsitio",info.Sitio },
                {"parmbdate",info.BirthDate },
                {"parmctznshp",info.Citizenship },
                {"parmbldType",info.BloodType },
                {"parmoccptn",info.Occupation },
                {"parmsklls",info.Skills },
                {"parmprfpic",info.ImageUrl },
                {"parmImgUrl",info.ImageUrl },
                {"parmusertype",info.AccountType },

                {"parmusername",info.Username },
                {"parmusrid",info.Userid},

                {"parmrlgn",info.Religion },
                {"parmhght",info.Height },
                {"parmwght",info.Weight+"kg" },
                {"parmbrtpl",info.Birthplace },

                {"parmfrfnm",info.Father?.Firstname ?? null},
                {"parmfrmnm",info.Father?.Middlename ?? null},
                {"parmfrlnm",info.Father?.Lastname ?? null},
                {"parmfrfllnm",info.Father?.Fullname ?? null},
                {"parmmrfnm",info.Mother?.Firstname ?? null},
                {"parmmrmnm",info.Mother?.Middlename ?? null},
                {"parmmrlnm",info.Mother?.Lastname ?? null},
                {"parmmrfllnm",info.Mother?.Fullname ?? null},
                {"parmspfnm",info.Spouse?.Firstname ?? null},
                {"parmspmnm",info.Spouse?.Middlename ?? null},
                {"parmsplnm",info.Spouse?.Lastname ?? null},
                {"parmspfllnm",info.Spouse?.Fullname ?? null},

                {"parmsplb",(info.IsParentLiveInBarangay) ? 1 : 0},
                {"parmslwp",(info.IsLivingWithParents) ? 1 : 0},
                {"parmssrctzn",(info.IsSeniorCitizen) ? 1 : 0},
                {"parmssp",(info.IsSingleParent) ? 1 : 0},
                {"parmsindgt",(info.IsIndigent) ? 1 : 0},
                {"parmspwd",(info.IsPWD) ? 1 : 0},
                {"parmrgstvtr",(info.IsRegisteredVoter) ? 1 : 0}

                //{"parmplid",membership.PLID },
                //{"parmpgrpid",membership.PGRPID },

                //{"parmrefgrpid",membership.GroupRef },
                //{"parmrefldrid", membership.SiteLeader },

                //{"parmfnm",membership.Firstname },
                //{"parmlnm",membership.Lastname },
                //{"parmmnm",membership.Middlename },
                //{"parmnnm",membership.Nickname },
                //{"parmmobno",membership.MobileNumber },
                //{"parmgender",membership.Gender },
                //{"parmmstastus",membership.MaritalStatus },
                //{"parmemladd",membership.EmailAddress },


                //{"parmprecentno", membership.PrecentNumber},
                //{"parmclusterno", membership.ClusterNumber },

                //{"parmhmeadd",membership.HomeAddress },
                //{"parmprsntadd",membership.PresentAddress },

                //{"parmreg",membership.Region },
                //{"parmprov",membership.Province },
                //{"parmmun",membership.Municipality },
                //{"parmbrgy",membership.Barangay },
                //{"parmsitio", membership.Sitio },

                //{"parmbdate",membership.BirthDate },
                //{"parmctznshp",membership.Citizenship },
                //{"parmbldType",membership.BloodType },
                //{"parmntnlty",membership.Nationality },
                //{"parmoccptn",membership.Occupation },
                //{"parmsklls",membership.Skills },
                //{"parmprfpic",membership.ImageUrl },
                //{"parmImgUrl",membership.ImageUrl },

                //{"parmusertype",membership.AccountType },
                //{"parmusername",membership.Username },
                ////{"parmpassword",membership.Userpassword },
                //{"parmusrid",membership.Userid },
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")

                    return (Results.Success, "Member Account Successful save");
                else if (ResultCode == "2")
                    return (Results.Failed, "Invalid Mobile Number");
                else if (ResultCode == "3")
                    return (Results.Failed, "Mobile Number already exist");
                else if (ResultCode == "4")
                    return (Results.Failed, "You are already Member of this Group");
                else if (ResultCode == "5")
                    return (Results.Failed, "Username already exist");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> GetBarangaySubscriberAsync(ResidentsInfo membership)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDBDAB0A", new Dictionary<string, object>()
            {
                {"parmreg", membership.Region },
                {"parmprov", membership.Province },
                {"parmmun", membership.Municipality },
                {"parmbrgy", membership.Barangay }
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if(ResultCode == "1")
                {
                    membership.PL_ID = row["PL_ID"].Str();
                    membership.PGRP_ID = row["PGRP_ID"].Str();
                    return (Results.Success, "Barangay Subscribe was activated.");
                }
                else if(ResultCode == "0")
                    return (Results.Failed, "Barangay Subscribe was not activated.");
            }
            return (Results.Null, null);
        }
    }
}
