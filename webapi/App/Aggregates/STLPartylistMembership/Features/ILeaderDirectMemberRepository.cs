using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.RequestModel.Common;
using webapi.Commons.AutoRegister;
using Comm.Commons.Extensions;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(LeaderDirectMemberRepository))]
    public interface ILeaderDirectMemberRepository
    {
        Task<(Results result, object member)> LoadMember(FilterRequest request);
        Task<(Results result, object member)> Load_Resident(FilterRequest request);
        Task<(Results result, object member)> LoadMember1(FilterRequest request);
        Task<(Results result, String message)> DirectAddMember(STLMembership membership, bool isUpdate = false);
        Task<(Results result, String message)> PromoteMembertoLeader(STLMembership request);
        Task<(Results result, String message)> ChangePassowrd(RequiredChangePassword request);
    }
    public class LeaderDirectMemberRepository : ILeaderDirectMemberRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public LeaderDirectMemberRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object member)> LoadMember(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BEABDBLLL0B2", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid", request.Userid},
                {"parmsrch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllMemberList(result.Read<dynamic>()));
            return (Results.Null, null);
        }
        public async Task<(Results result, object member)> LoadMember1(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BEABDBLLL0B4", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmusrid", request.Userid},
                {"parmrownum", request.num_row},
                {"parmsrch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllMemberList(result.Read<dynamic>(),request.Userid,100));
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> DirectAddMember(STLMembership membership, bool isUpdate = false)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_BDABDBCAACBB06", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },

                {"parmldrtype",membership.Type },
                {"parmsubtyp",membership.SubType },

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

                {"parmprecentno", membership.PrecentNumber},
                {"parmclusterno", membership.ClusterNumber },

                {"parmhmeadd",membership.HomeAddress },
                {"parmprsntadd",membership.PresentAddress },

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

                {"parmusrid",(isUpdate?membership.Userid:"") },
                {"parmoptrid",account.USR_ID },
            }).ReadSingleOrDefault();
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results);
                string ResultCode = row["RESULT"].Str();
                if(ResultCode=="1")
                    return (Results.Success, "Succesfull save");
                else if (ResultCode == "2")
                    return (Results.Failed, "Invalid Mobile Number");
                else if (ResultCode == "3")
                    return (Results.Failed, "Username already exist");
                else if (ResultCode == "4")
                    return (Results.Failed, "You are already Member of this Group");
                else if (ResultCode == "5")
                    return (Results.Failed, "Username already exist");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> PromoteMembertoLeader(STLMembership request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BEA0A", new Dictionary<string, object>()
            {
                {"parmplid",request.PL_ID },
                {"parmpgrpid",request.PGRP_ID },
                {"parmoptid", account.USR_ID },
                {"parmusrid", request.Userid },
                {"parmacctid",request.ACT_ID },
                {"parmldrtype",request.Type },
                {"parmrefgrpid",request.GroupRef },
                {"parmlocsite",request.LocationSite }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Succesfull Promoted");
                else if (ResultCode == "0")
                    return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string message)> ChangePassowrd(RequiredChangePassword request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDA0A", new Dictionary<string, object>()
            {
                {"parmplid",request.PLID },
                {"parmpgrpid",request.PGRPID },
                {"parmusrid", account.USR_ID },
                {"parmoldpassword",request.OldPassword },
                {"parmnewpassword",request.Password },
                {"parmconfirmpassword",request.ConfirmPassword }
            }).FirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                var ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                    return (Results.Success, "Change Successfull! you can now use your new password");
                else if (ResultCode == "61")
                    return (Results.Failed, "Password did not match");
                else if (ResultCode == "62")
                    return (Results.Failed, "You are trying to user your old password, please try again.");
                else if (ResultCode == "0")
                    return (Results.Failed, "Your username or mobile number was not exist, please try again.");
                else if (ResultCode == "2")
                    return (Results.Failed, "You entered wrong password, please try again.");
                else if (ResultCode == "21")
                    return (Results.Failed, "You are try to access block account, please try again.");
                return (Results.Null, "Failed to Change! your request is already done");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object member)> Load_Resident(FilterRequest request)
        {
            var result = _repo.DSpQuery<dynamic>($"dbo.spfn_BDABDB060A", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
                {"parmsubtype", account.SUB_TYP },
                {"parmrownum",request.num_row },
                {"parmsrch",request.Search },
                {"parmreg",request.Region },
                {"parmprov",request.Province },
                {"parmmun",request.Municipality },
                {"parmbrgy",request.Barangay },
                {"parmsitio","" }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllRespondentList(result, request.Userid, 100));
            return (Results.Null, null);
        }
    }
}
