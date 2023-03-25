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
        //Task<(Results result, String message, STLSignInRequest signin, STLAccount account)> MembershipAsync(STLMembership membership, bool isUpdate = false);
        Task<(Results result, String message)> MembershipAsync(STLMembership membership, bool isUpdate = false);

        Task<(Results result, String message)> UpdateMembershipAsync(STLMembership membership);
        Task<(Results result, object bryoptr)> Load_BrgyOperator(FilterRequest req);
    }
    public class STLMembershipRepository : ISTLMembershipRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public STLMembershipRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }


        public async Task<(Results result, object bryoptr)> Load_BrgyOperator(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BDB0E", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmsearch", req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetBrgyOperatorList(result.Read<dynamic>(), 1000));

            return (Results.Null, null);
        }
        //public async Task<(Results result, string message, STLSignInRequest signin, STLAccount account)> MembershipAsync(STLMembership membership, bool isUpdate = false)
        public async Task<(Results result, string message)> MembershipAsync(STLMembership membership, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDABDBCAACBB01", new Dictionary<string, object>()
            {
                {"parmplid",membership.PL_ID },
                {"parmpgrpid",membership.PGRP_ID },
                {"parmrefgrpid",membership.GroupRef },
                {"parmrefldrid",membership.SiteLeader },
                {"parmfnm",membership.Firstname },
                {"parmlnm",membership.Lastname },
                {"parmmnm",membership.Middlename },
                {"parmnnm",membership.Nickname },
                {"parmmobno",membership.MobileNumber },
                {"parmgender",membership.Gender },
                {"parmmstastus",membership.MaritalStatus },
                {"parmemladd",membership.EmailAddress },
                {"parmhmeadd",membership.HomeAddress },
                {"parmprsntadd",membership.PresentAddress },
                {"parmprecentno", membership.PrecentNumber},
                {"parmclusterno", membership.ClusterNumber },
                {"parmreg",membership.Region },
                {"parmprov",membership.Province },
                {"parmmun",membership.Municipality },
                {"parmbrgy",membership.Barangay },
                {"parmsitio",membership.Sitio },
                {"parmbdate",membership.BirthDate },
                {"parmctznshp",membership.Citizenship },
                {"parmbldType",membership.BloodType },
                {"parmntnlty",membership.Nationality },
                {"parmoccptn",membership.Occupation },
                {"parmsklls",membership.Skills },
                {"parmprfpic",membership.ImageUrl },
                {"parmImgUrl",membership.ImageUrl },
                {"parmusertype",membership.AccountType },

                {"parmusername",membership.Username },
                //{"parmpassword",membership.Userpassword },
                {"parmusrid",(isUpdate?membership.Userid:"") },
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
                    return (Results.Success, "Member Account Successful save, login using your register mobile number and temporary password 123456");
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

        public async Task<(Results result, string message)> UpdateMembershipAsync(STLMembership membership)
        {
            bool isleader = Convert.ToBoolean(membership.isLeader);
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDABDBCAACBB05", new Dictionary<string, object>()
            {
                {"parmplid",membership.PL_ID },
                {"parmpgrpid",membership.PGRP_ID },

                {"parmrefgrpid",membership.GroupRef },
                {"parmrefldrid", membership.SiteLeader },

                {"parmfnm",membership.Firstname },
                {"parmlnm",membership.Lastname },
                {"parmmnm",membership.Middlename },
                {"parmnnm",membership.Nickname },
                {"parmmobno",membership.MobileNumber },
                {"parmgender",membership.Gender },
                {"parmmstastus",membership.MaritalStatus },
                {"parmemladd",membership.EmailAddress },


                {"parmprecentno", membership.PrecentNumber},
                {"parmclusterno", membership.ClusterNumber },

                {"parmhmeadd",membership.HomeAddress },
                {"parmprsntadd",membership.PresentAddress },

                {"parmreg",membership.Region },
                {"parmprov",membership.Province },
                {"parmmun",membership.Municipality },
                {"parmbrgy",membership.Barangay },
                {"parmsitio", membership.Sitio },

                {"parmbdate",membership.BirthDate },
                {"parmctznshp",membership.Citizenship },
                {"parmbldType",membership.BloodType },
                {"parmntnlty",membership.Nationality },
                {"parmoccptn",membership.Occupation },
                {"parmsklls",membership.Skills },
                {"parmprfpic",membership.ImageUrl },
                {"parmImgUrl",membership.ImageUrl },

                {"parmusertype",membership.AccountType },
                {"parmusername",membership.Username },
                //{"parmpassword",membership.Userpassword },
                {"parmusrid",membership.Userid },
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
    }
}
