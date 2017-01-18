using System;
using System.IO;
using System.Linq;
using System.Text;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using servicedesk.Common.Commands;
using servicedesk.Common.Events;

namespace servicedesk.Common.Host
{
    public class WebServiceHost : IWebServiceHost
    {
        private readonly IWebHost _webHost;

        public WebServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public void Run()
        {
            _webHost.Run();
        }

        public static Builder Create<TStartup>(string name = "", int port = 80) where TStartup : class
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = $"Coolector Service: {typeof(TStartup).Namespace.Split('.').Last()}";
            }            

            Console.Title = name;
            var webHost = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<TStartup>()
                .UseUrls($"http://*:{port}")
                .Build();
            var builder = new Builder(webHost);

            return builder;
        }

        public abstract class BuilderBase
        {
            public abstract WebServiceHost Build();
        }

        public class Builder : BuilderBase
        {
            private IResolver _resolver;
            private IModel _bus;
            private readonly IWebHost _webHost;

            public Builder(IWebHost webHost)
            {
                _webHost = webHost;
            }

            public Builder UseAutofac(ILifetimeScope scope)
            {
                _resolver = new AutofacResolver(scope);

                return this;
            }

            public BusBuilder UseRabbitMq(string exchangeName = null)
            {
                _bus = _resolver.Resolve<IModel>();

                return new BusBuilder(_webHost, _bus, _resolver, exchangeName);
            }

            public override WebServiceHost Build()
            {
                return new WebServiceHost(_webHost);
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private readonly IModel _bus;
            private readonly IResolver _resolver;
            private readonly string _exchangeName;

            public BusBuilder(IWebHost webHost, IModel bus, IResolver resolver, string exchangeName = null)
            {
                _webHost = webHost;
                _bus = bus;
                _resolver = resolver;
                _exchangeName = exchangeName;
                
                _bus.ExchangeDeclare(exchange: _exchangeName, type: "topic");
            }

            public BusBuilder SubscribeToCommand<TCommand>(string queueName) where TCommand : ICommand
            {
                var queue = _bus.QueueDeclare(queueName);
                _bus.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

                var consumer = new EventingBasicConsumer(_bus);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var @command = Newtonsoft.Json.JsonConvert.DeserializeObject<TCommand>(message);
                    var commandHandler = _resolver.Resolve<ICommandHandler<TCommand>>();

                    commandHandler.HandleAsync(@command);
                };

                _bus.BasicConsume(queue: queue, noAck: true, consumer: consumer);

                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>(string queueName) where TEvent : IEvent
            {                
                var queue = _bus.QueueDeclare(queueName);
                _bus.QueueBind(queue: queueName, exchange: "logs", routingKey: "");

                var consumer = new EventingBasicConsumer(_bus);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    var @event = Newtonsoft.Json.JsonConvert.DeserializeObject<TEvent>(message);
                    var eventHandler = _resolver.Resolve<IEventHandler<TEvent>>();

                    eventHandler.HandleAsync(@event);
                };

                _bus.BasicConsume(queue: queue, noAck: true, consumer: consumer);

                return this;
            }

            public override WebServiceHost Build()
            {
                return new WebServiceHost(_webHost);
            }
        }
    }
}