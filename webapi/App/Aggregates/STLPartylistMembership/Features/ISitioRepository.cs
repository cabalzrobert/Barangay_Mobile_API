using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.App.Aggregates.Common;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;
using webapi.App.Model.User;
using Comm.Commons.Extensions;
using webapi.Commons.AutoRegister;
using webapi.App.RequestModel.AppRecruiter;
using webapi.App.Aggregates.Common.Dto;
using webapi.App.RequestModel.Common;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(SitioRepository))]
    public interface ISitioRepository
    {
        Task<(Results result, object sit)> LoadSitio(Sitio sit);
        Task<(Results result, object brgy)> LoadBarangay(Barangay brgy);
    }
    public class SitioRepository : ISitioRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public SitioRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, object sit)> LoadSitio(Sitio sit)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_LLL0C",new Dictionary<string, object>()
            {
                {"parmbrgy",sit.ID }
            });
            if (result != null)
            {
                return (Results.Success, STLSubscriberDto.GetSitioList(result.Read<dynamic>()));
            }
            return (Results.Null, null);
        }

        public async Task<(Results result, object brgy)> LoadBarangay(Barangay brgy)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGY0C", new Dictionary<string, object>()
            {
                {"@parmcode",brgy.ID }
            });
            if (result != null)
            {
                return (Results.Success, STLSubscriberDto.GetBrgyList(result.Read<dynamic>()));
            }
            return (Results.Null, null);
        }

    }
}
