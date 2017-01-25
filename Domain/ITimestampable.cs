using System;

namespace servicedesk.Common.Domain
{
    public interface ITimestampable
    {
        DateTime CreatedAt { get; }
    }
}