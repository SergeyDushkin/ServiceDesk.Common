using System;
using RawRabbit.Attributes;
using RawRabbit.Configuration.Exchange;

namespace servicedesk.Common.Events
{
    [Queue(Name = "setnewstatusrejected", MessageTtl = 300, DeadLeterExchange = "dlx", Durable = false)]
    [Exchange(Name = "servicedesk.statusmanagementsystem.events", Type = ExchangeType.Topic)] 
    public class SetNewStatusRejected : IRejectedEvent
    {
        public Guid RequestId { get; }
        public string UserId { get; }
        public string Reason { get; }
        public string Code { get; }

        protected SetNewStatusRejected()
        {
        }

        public SetNewStatusRejected(Guid requestId, string code, string reason)
        {
            RequestId = requestId;
            Reason = reason;
            Code = code;
        }
    }
}