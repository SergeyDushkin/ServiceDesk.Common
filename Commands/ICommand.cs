namespace servicedesk.Common.Commands
{
    public interface ICommand
    {
        Request Request { get; set; }
    }
}
