using RawRabbit;
using RawRabbit.Common;
using servicedesk.Common.Commands;
using servicedesk.Common.Events;

namespace servicedesk.Common.Extensions
{
    public static class RawRabbitExtensions
    {
        /*
        public static ISubscription WithCommandHandlerAsync<TCommand>(this IBusClient bus,
            ICommandHandler<TCommand> handler, string name = null) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>(async (msg, context) => await handler.HandleAsync(msg)); 

        public static ISubscription WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IEventHandler<TEvent> handler, string name = null) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(async (msg, context) => await handler.HandleAsync(msg));
        */
        
        public static ISubscription WithCommandHandlerAsync<TCommand>(this IBusClient bus,
            ICommandHandler<TCommand> handler, string exchangeName = null, string routingKey = null) where TCommand : ICommand
            => bus.SubscribeAsync<TCommand>(async (msg, context) => await handler.HandleAsync(msg),
            cfg => cfg.WithExchange(e => e.WithName(exchangeName)).WithRoutingKey(routingKey));
            
          //  .WithQueue(q => q.WithName(name))); //GetExchangeName<TCommand>(name)

        public static ISubscription WithEventHandlerAsync<TEvent>(this IBusClient bus,
            IEventHandler<TEvent> handler, string exchangeName = null, string routingKey = null) where TEvent : IEvent
            => bus.SubscribeAsync<TEvent>(async (msg, context) => await handler.HandleAsync(msg),
            cfg => cfg.WithExchange(e => e.WithName(exchangeName)).WithRoutingKey(routingKey));

            /*
        private static string GetExchangeName<T>(string name = null)
            => string.IsNullOrWhiteSpace(name)
                ? $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}"
                : $"{name}/{typeof(T).Name}";*/
    }
}