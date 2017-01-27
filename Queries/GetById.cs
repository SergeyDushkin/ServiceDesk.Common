using System;

namespace servicedesk.Common.Queries
{
    public class GetById : IQuery
    {
        public Guid Id { get; set; }
    }
}