using System;
using System.Threading.Tasks;

namespace servicedesk.Common.Services
{
    public interface IHandler
    {
        IHandlerTask Run(Action action);
        IHandlerTask Run(Func<Task> actionAsync);
        void ExecuteAll();
        Task ExecuteAllAsync();
    }
}