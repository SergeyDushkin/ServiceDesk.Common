using System;

namespace servicedesk.Common.Commands
{
public class Request
{
    public DateTime CreatedAt { get; set; }
    public string Culture { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Origin { get; set; }
    public string Resource { get; set; }
}

public class SetStatus
{
    public Request Request { get; set; }
    public Guid SourceId { get; set; }
    public Guid ReferenceId { get; set; }
    public Guid StatusId { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
}
}
