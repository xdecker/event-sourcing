using System;
using System.Text.Json;
using Common.Core.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Ticketing.Query.Domain.Abstraction;
using Ticketing.Query.Infrastructure.Converters;

namespace Ticketing.Query.Infrastructure.Consumers;

//esto va analizando en background
public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ConsumerConfig _config;

    public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceProvider serviceProvider, IOptions<ConsumerConfig> config)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _config = config.Value;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("El event consumer service esta trabajando");
        var topic = "KAFKA_TOPIC";

        //crear el consumer
        using var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        //suscribirte
        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();
            if (consumeResult is null) continue;

            if (consumeResult.Message is null) continue;

            var options = new JsonSerializerOptions
            { Converters = { new EventJsonConverter() } };

            var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);

            if (@event is null)
                throw new ArgumentNullException(
                    nameof(@event), "No se pudo procesar el json parse"
                );

            using IServiceScope scope = _serviceProvider.CreateScope();
            var _eventHandler = scope.ServiceProvider.GetRequiredService<IEventHandler>();
            var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });
            if (handlerMethod is null)
            {
                throw new ArgumentNullException(nameof(handlerMethod), "No encontró el metodo handler correspondiente");
            }

            handlerMethod.Invoke(_eventHandler, new object[] { @event });


            //quitar del listado el consumido
            consumer.Commit(consumeResult);

        }

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
