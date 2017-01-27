using System;

namespace servicedesk.Common.Domain
{
    public interface IDependently
    {
        Guid ReferenceId { get; }
    }
}