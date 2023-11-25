using System;

namespace webapi.App.RequestModel.Feature
{
    public class ConnectionRequest
    {
        public string ConnectionRequestId;
        public string Agenda;
        public bool IsAccepted;
        public bool IsDeclined;
        public bool IsCanceled;
        public bool IsUpdate;
    }
}
