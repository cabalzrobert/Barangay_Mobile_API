using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Repositories;
using webapi.App.Model.User;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.Common.Dto;
using webapi.Commons.AutoRegister;
using webapi.App.RequestModel.AppRecruiter;
using Comm.Commons.Extensions;
using webapi.App.Features.UserFeature;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(PostCommunityRepository))]
    public interface IPostCommunityRepository
    {
        Task<(Results result, object comm)> LoadPostCommunityListAsync(FilterRequest req);
        Task<(Results result, object comment)> LoadCommentPostCommunityListAsync(FilterRequest req);
        Task<(Results result, String message)> SendRequestJoinCommunityAsync(Community req);
        Task<(Results result, String message)> SendPostCommentAsync(PostCommentCommunity req);
        Task<(Results result, String CountCommunity)> GetCountCommunityAsync();
        Task<(Results result, object comm)> LoadCommunityListAsync(FilterRequest req);
        Task<(Results result, string Message)> ReactionPostCommunityAsync(PostCommunityReaction req);
        Task<(Results result, string Message)> ReactionCommentPostCommunityAsync(CommentPostCommunityReaction req);
        Task<(Results result, object commentnotifcation)> ViewCommentPostCommunityAsync(CommentPostCommunity req);
    }
    public class PostCommunityRepository : IPostCommunityRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public PostCommunityRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object comm)> LoadPostCommunityListAsync(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRAC000P4", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
                {"parmrownum",req.num_row },
                {"parmcommid",req.CommunityID },
                {"parmfilter", req.FilterBy },
                {"parmsearch", req.Search }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllPostCommunityList(result.Read<dynamic>(), req.Userid, 25));
            return (Results.Null, null);
        }

        public async Task<(Results result, object comment)> LoadCommentPostCommunityListAsync(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRACCOM0003", new Dictionary<string, object>()
            {
                {"parmcommid",req.CommunityID },
                {"parmpostid",req.PostID },
                {"parmrownum",req.NextFilter },
                {"parmuserid", account.USR_ID }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllCommentPostCommunityList(result.Read<dynamic>(), req.Userid, 30));
            return (Results.Null, null);
        }



        public async Task<(Results result, string message)> SendRequestJoinCommunityAsync(Community req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRAC000A3", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmcommunityjson",req.CommunityJson },
                {"parmuserid",account.USR_ID },
            }).ReadFirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    return (Results.Success, "Join community successful. Start exploring!");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Please check your data entry. Please try again");
                return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }
        public async Task<(Results result, string message)> SendPostCommentAsync(PostCommentCommunity req)
        {
            req.CommentID = ((int)DateTime.Now.ToTimeMillisecond()).ToString("X");
            req.Post_Date = DateTime.Now.Str();
            req.CommenterName = account.FLL_NM;
            req.USR_ID = account.USR_ID;
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRACCOM0001", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmcommid",req.CommunityID },
                {"parmpostid",req.PostID },
                {"parmcommentid",req.CommentID },
                {"parmcommentdescription",req.CommentDescription },
                {"parmuserid",account.USR_ID },
            }).ReadFirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    await notifyCommentPostCommunity(req);
                    return (Results.Success, "Post comment successful.");
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Please check your data entry. Please try again");
                return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string CountCommunity)> GetCountCommunityAsync()
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRAC000J1", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmuserid",account.USR_ID },
            }).ReadFirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode != "0")
                {
                    return (Results.Success, ResultCode);
                }
                else
                {
                    return (Results.Failed, ResultCode);
                }
            }
            return (Results.Null, "0");
        }

        public async Task<(Results result, object comm)> LoadCommunityListAsync(FilterRequest req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRAC000B2", new Dictionary<string, object>()
            {
                {"parmuserid",account.USR_ID },
                {"parmrownum",req.num_row }
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllCommunitiesList(result.Read<dynamic>(), req.Userid, 30));
            return (Results.Null, null); throw new NotImplementedException();
        }

        public async Task<(Results result, object commentnotifcation)> ViewCommentPostCommunityAsync(CommentPostCommunity req)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRACPOSTCOM0001", new Dictionary<string, object>()
            {
                {"parmcommunityid",req.CommunityID },
                {"parmpostid",req.PostID },
                {"parmcommentid",req.CommentID },
                {"parmuserid",account.USR_ID },
            });
            if (result != null)
            {
                return (Results.Success, STLSubscriberDto.GetCommentPostCommunityView(result.Read<dynamic>(), account.USR_ID, 30));
            }
            return (Results.Null, null);
        }


        public async Task<(Results result, string Message)> ReactionPostCommunityAsync(PostCommunityReaction req)
        {
            req.USR_ID = account.USR_ID;
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRACPRXN0001", new Dictionary<string, object>()
            {
                {"parmcommid",req.CommunityID },
                {"parmpostid",req.PostID },
                {"parmuserid",account.USR_ID },
                {"parmlike",req.isLike },
                {"parmdislike",req.isDislike },
            }).ReadFirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (req.isLike == 1 && req.isDislike == 0)
                    {
                        await notifyReactionPostCommunity(req);
                        return (Results.Success, "You like this post.");
                    }
                    else if(req.isLike == 0 && req.isDislike == 1)
                    {
                        await notifyReactionPostCommunity(req);
                        return (Results.Success, "You dis-like this post.");
                    }
                    else if (req.isLike == 0 && req.isDislike == 0)
                    {
                        await notifyReactionPostCommunity(req);
                        return (Results.Success, "");
                    }
                        
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Please check your data entry. Please try again");
                return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, string Message)> ReactionCommentPostCommunityAsync(CommentPostCommunityReaction req)
        {
            req.USR_ID = account.USR_ID;
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BIMSRACCRXN0001", new Dictionary<string, object>()
            {
                {"parmcommid",req.CommunityID },
                {"parmpostid",req.PostID },
                {"parmcommentid",req.CommentID },
                {"parmuserid",account.USR_ID },
                {"parmlike",req.isLike },
                {"parmdislike",req.isDislike },
            }).ReadFirstOrDefault();
            if (result != null)
            {
                var row = ((IDictionary<string, object>)result);
                string ResultCode = row["RESULT"].Str();
                if (ResultCode == "1")
                {
                    if (req.isLike == 1 && req.isDislike == 0)
                    {
                        await notifyReactionCommentPostCommunity(req);
                        return (Results.Success, "You like this commentt.");
                    }
                        
                    else if (req.isLike == 0 && req.isDislike == 1)
                    {
                        await notifyReactionCommentPostCommunity(req);
                        return (Results.Success, "You dis-like this post.");
                    }
                        
                    else if (req.isLike == 0 && req.isDislike == 0)
                    {
                        await notifyReactionCommentPostCommunity(req);
                        return (Results.Success, "");
                    }
                        
                }
                else if (ResultCode == "2")
                    return (Results.Failed, "Please check your data entry. Please try again");
                return (Results.Failed, "Somethings wrong in your data. Please try again");
            }
            return (Results.Null, null);
        }


        private async Task<bool> notifyCommentPostCommunity(PostCommentCommunity req)
        {
            await Pusher.PushAsync($"/{account.PL_ID}/commentpostcommunity", new { type = "commentpostcommunity", content = req });
            return true;
        }
        private async Task<bool> notifyReactionPostCommunity(PostCommunityReaction req)
        {
            await Pusher.PushAsync($"/{account.PL_ID}/postcommunity/reaction", new { type = "postcommunity", content = req });
            return true;
        }
        private async Task<bool> notifyReactionCommentPostCommunity(CommentPostCommunityReaction req)
        {
            await Pusher.PushAsync($"/{account.PL_ID}/postcommunity/comment/reaction", new { type = "commentpostcommunity", content = req });
            return true;
        }

        
    }
}
