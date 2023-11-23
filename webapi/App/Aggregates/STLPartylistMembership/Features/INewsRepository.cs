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
                {"parmcategory", req.Category},
                {"parmrownum",req.num_row },
                {"parmsearch",req.Search }
            });
            if (results != null)
                return (Results.Success, STLSubscriberDto.GetNewsList(results.Read<dynamic>(), account.USR_ID, 100));
            //return (Results.Success, results);
            return (Results.Null, null);
        }
    }
}
