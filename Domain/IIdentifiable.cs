using System;

namespace servicedesk.Common.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}