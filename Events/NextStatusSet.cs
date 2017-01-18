using System;
using Coolector.Common.Events;
using RawRabbit.Attributes;
using RawRabbit.Configuration.Exchange;

namespace servicedesk.Common.Events
{
    [Queue(Name = "nextstatusset", MessageTtl = 300, DeadLeterExchange = "dlx", Durable = false)]
    [Exchange(Name = "servicedesk.statusmanagementsystem.events", Type = ExchangeType.Topic)] 
    public class NextStatusSet : IEvent
    {
        public Guid RequestId { get; }
        public Guid SourceId { get; }
        public Guid ReferenceId { get; }
        public Guid StatusId { get; }

        protected NextStatusSet()
        {
        }

        public NextStatusSet(Guid requestId, Guid sourceId, Guid referenceId, Guid statusId)
        {
            RequestId = requestId; 
            SourceId = sourceId; 
            ReferenceId = referenceId;
            StatusId = statusId;
        }
    }
}