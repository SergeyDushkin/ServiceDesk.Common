using System;
using servicedesk.Common.Types;

namespace servicedesk.Common.Queries
{
    public class GetByReferenceId : PagedQueryBase
    {
        public Guid ReferenceId { get; set; }
    }
}