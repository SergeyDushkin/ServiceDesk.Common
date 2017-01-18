using System;

namespace servicedesk.Common.Events
{
    public interface IEvent
    {
        Guid RequestId { get; }
    }
}