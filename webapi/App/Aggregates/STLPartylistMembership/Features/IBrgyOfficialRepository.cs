using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Comm.Commons.Extensions;
using Infrastructure.Repositories;
using webapi.Commons.AutoRegister;
using webapi.App.Aggregates.Common;
using webapi.App.RequestModel.SubscriberApp;
using webapi.App.RequestModel.SubscriberApp.Common;
using webapi.App.Model.User;
using webapi.App.Globalize.Company;

using Newtonsoft.Json;
using System.IO;
using webapi.App.Aggregates.SubscriberAppAggregate.Common;

using Comm.Commons.Advance;
using webapi.App.RequestModel.Feature;
using webapi.Services.Firebase;
using webapi.Services.Dependency;
using System.Net;

using System.Net.Mail;
using webapi.App.RequestModel.Common;
using webapi.App.Aggregates.Common.Dto;

namespace webapi.App.Aggregates.STLPartylistMembership.Features
{
    [Service.ITransient(typeof(BrgyOfficialRepository))]
    public interface IBrgyOfficialRepository
    {
        Task<(Results result, object brgyofficial)> Load_BrgyOfficial(FilterRequest request);
    }
    public class BrgyOfficialRepository: IBrgyOfficialRepository
    {
        private readonly ISubscriber _identity;
        private readonly IRepository _repo;
        private readonly IFileData _fd;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public BrgyOfficialRepository(ISubscriber identity, IRepository repo, IFileData fd)
        {
            _identity = identity;
            _repo = repo;
            _fd = fd;
        }

        public async Task<(Results result, object brgyofficial)> Load_BrgyOfficial(FilterRequest request)
        {
            var result = _repo.DSpQueryMultiple($"dbo.spfn_BRGYOFLBDB0A", new Dictionary<string, object>()
            {
                {"parmplid",account.PL_ID },
                {"parmpgrpid",account.PGRP_ID },
                {"parmstatus", request.Status},
                {"parmrownum", request.num_row},
                {"parmsearch", request.Search}
            });
            if (result != null)
                return (Results.Success, STLSubscriberDto.GetAllBrgyOfficialList(result.Read<dynamic>(), request.Userid, 100));
            return (Results.Null, null);
        }
    }
}
