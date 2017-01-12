using System;
using Coolector.Common.Events;

namespace servicedesk.Common.Events
{
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