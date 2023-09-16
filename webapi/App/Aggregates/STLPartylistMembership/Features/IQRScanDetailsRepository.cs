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
    [Service.ITransient(typeof(QRScanDetailsRepository))]
    public interface IQRScanDetailsRepository
    {
        Task<(Results result, string message, object data)> getQRScannedDetails(string scanId);
    }
    public class QRScanDetailsRepository : IQRScanDetailsRepository
    {
        private readonly ISubscriber _identity;
        public readonly IRepository _repo;
        public STLAccount account { get { return _identity.AccountIdentity(); } }
        public QRScanDetailsRepository(ISubscriber identity, IRepository repo)
        {
            _identity = identity;
            _repo = repo;
        }

        public async Task<(Results result, string message, object data)> getQRScannedDetails(string scanId)
        {
            var results = _repo.DSpQueryMultiple("dbo.spfn_BIMSSQRPRFDTL", new Dictionary<string, object>()
            {
                {"parmusrid", scanId }
            });
            if (results != null)
            {
                var row = ((IDictionary<string, object>)results.ReadFirstOrDefault());
                if (row != null)
                    return (Results.Success, null, new
                    {
                        USR_ID = row["USR_ID"].Str(),
                        FLL_NM = row["FLL_NM"].Str(),
                        AGE = getAge(row["BRT_DT"].Str()),
                        BRT_DT = row["BRT_DT"].Str(),
                        GNDR = row["GNDR"].Str() == "m" ? "MALE":"FEMALE",
                        HEIGHT = row["HEIGHT"].Str(),
                        WEIGHT = row["WEIGHT"].Str(),
                        PRSNT_ADDR = row["PRSNT_ADDR"].Str(),
                        PLC_BRT = row["PLC_BRT"].Str(),
                        PRF_PIC = row["PRF_PIC"].Str(),
                        BRGY = row["BRGY"].Str(),
                        SIT_NM = row["SIT_NM"].Str(),
                        RLGN = row["DESCRIPTION"].Str()
                    });
            }
            return (Results.Failed, "Invalid QR Code", null);
        }

        private int getAge(string bday)
        {
            int age = 0;
            age = Convert.ToInt32((DateTime.Now.Subtract(DateTime.Parse(bday)).TotalDays / 365));
            return age;
        }

    }
}
