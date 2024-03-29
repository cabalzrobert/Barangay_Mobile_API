using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.Model.User;
using webapi.App.Globalize.Company;

using Newtonsoft.Json;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

namespace webapi.App.Aggregates.SubscriberAppAggregate.Features
{
    [Service.ITransient(typeof(NotificationRepository))] 
    public interface INotificationRepository
    {
        Task<(Results result, object items)> NotificationAsync(FilterRequest filter);
        Task<(Results result, object items)> AnnouncementAsync(FilterRequest filter);
        Task<(Results result, object obj)> SeenAsync(String NotificationID);
        Task<(Results result, object count)> UnseenCountAsync();
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        public STLAccount account { get{ return _identity.AccountIdentity(); } } 
        public NotificationRepository(ISubscriber identity, IRepository repo){
            _identity = identity; 
            _repo = repo; 
        }

        public async Task<(Results result, object items)> NotificationAsync(FilterRequest filter){
            var results = _repo.DSpQueryMultiple("dbo.spfn_0AA0AB0A", new Dictionary<string, object>(){
                { "parmplid", account.PL_ID },
                { "parmpgrpid", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmftrns", filter.BaseFilter },
                { "parmmemberofcommunity", filter.MemberofCommunity },
            });
            if(results != null)
                return (Results.Success, NotificationDto.FilterNotifications(results.Read(), 25));
            return (Results.Null, null); 
        }
        public async Task<(Results result, object obj)> SeenAsync(String NotificationID){
            var result = _repo.DSpQuery<dynamic>("dbo.spfn_0AA0AB0B", new Dictionary<string, object>(){
                { "parmplid", account.PL_ID },
                { "parmpgrpid", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmnotifid", NotificationID }, 
            });
            return (Results.Success, null); 
        }
        public async Task<(Results result, object count)> UnseenCountAsync(){
            var result = _repo.DSpQueryMultiple("dbo.spfn_0AA0AB0C", new Dictionary<string, object>(){
                { "parmplid", account.PL_ID },
                { "parmpgrpid", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
            }).ReadSingleOrDefault();
            if(result != null){
                var row = ((IDictionary<string, object>) result);
                return (Results.Success, row["UN_OPN"].Str());
            }
            return (Results.Null, null); 
        }

        public async Task<(Results result, object items)> AnnouncementAsync(FilterRequest filter)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_0AA0AB0D", new Dictionary<string, object>(){
                { "parmplid", account.PL_ID },
                { "parmpgrpid", account.PGRP_ID },
                { "parmuserid", account.USR_ID },
                { "parmstartdate", filter.StartDate },
                { "parmenddate", filter.EndDate },
                { "parmrownum", filter.num_row },
                //{ "parmftrns", filter.BaseFilter },
                //{ "parmmemberofcommunity", filter.MemberofCommunity },
            });
            if (results != null)
                return (Results.Success, NotificationDto.FilterAnnouncements(results.Read(), 25));
            return (Results.Null, null);
        }
    }
}