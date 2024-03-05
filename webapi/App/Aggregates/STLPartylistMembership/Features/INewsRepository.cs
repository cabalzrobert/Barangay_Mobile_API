using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.RequestModel.Common;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using Infrastructure.Repositories;
using webapi.App.Model.User;
using webapi.App.Aggregates.Common.Dto;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(NewsRepository))]
    public interface INewsRepository
    {
        Task<(Results result, object item)> Load_NewsAsync(FilterRequest req);
        Task<(Results result, object item)> Load_CategoryAsync();
    }
    public class NewsRepository : INewsRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public NewsRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object item)> Load_NewsAsync(FilterRequest req)
        {
            var results = _repo.DSpQueryMultiple($"dbo.BIMSSNEWS0AA0A", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid", account.PGRP_ID},
                {"parmcategory", req.Category},
                {"parmrownum",req.num_row },
                {"parmstartdate", req.StartDate },
                {"parmenddate", req.EndDate },
                {"parmsearch",req.Search }
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetNewsList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }

        public async Task<(Results result, object item)> Load_CategoryAsync()
        {
            var results = _repo.DSpQueryMultiple($"dbo.spfn_BIMSSNEWS0AA0B", new Dictionary<string, object>()
            {
                {"parmplid", account.PL_ID},
                {"parmpgrpid",account.PGRP_ID },
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetNewsCategoryList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }
    }
}
