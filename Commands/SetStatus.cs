using System;
using Coolector.Common.Commands;
using RawRabbit.Attributes;
using RawRabbit.Configuration.Exchange;

namespace servicedesk.Common.Commands
{
    [Queue(Name = "setstatus", MessageTtl = 300, DeadLeterExchange = "dlx", Durable = false)]
    [Exchange(Name = "servicedesk.statusmanagementsystem.commands", Type = ExchangeType.Topic)]    
    public class SetStatus : IAuthenticatedCommand
    {
        public Request Request { get; set; }
        public Guid SourceId { get; set; }
        public Guid ReferenceId { get; set; }
        public Guid StatusId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}
