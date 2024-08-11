using System.Globalization;
using AutoFixture;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DeviceTelemetry.Domain.Entities;
using DeviceTelemetry.Infrastructure.Constants;

using Telemetry = DeviceTelemetry.Domain.Entities.Telemetry;

namespace DeviceTelemetry.Producer.BackgroundServices;

public class TelemetryProducer : BackgroundService
{
    private readonly CosmosClient _cosmosClient;
    private readonly IFixture _fixture;
    private readonly ILogger<TelemetryProducer> _logger;

    public TelemetryProducer(
        CosmosClient cosmosClient, 
        IFixture fixture,
        ILogger<TelemetryProducer> logger)
    {
        _cosmosClient = cosmosClient;
        _fixture = fixture;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            var messages = GenerateTelemetryMessages();
            await SendTelemetryToCosmos(messages);
            
            _logger.LogInformation("Sent telemetry to Cosmos DB");
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
    
    private async Task SendTelemetryToCosmos(IEnumerable<Telemetry> messages)
    {
        var container = _cosmosClient.GetContainer(
            CosmosDbConstants.DatabaseNames.DeviceTelemetry, 
            CosmosDbConstants.ContainerNames.Telemetry);
        foreach (var message in messages)
        {
            await container.CreateItemAsync(message);
        }
    }

    private IEnumerable<Telemetry> GenerateTelemetryMessages()
    {
        var deviceIdBuilder = new ElementsBuilder<Guid>(
            Guid.Parse("6d76a1ab-89af-4e30-aa73-f52a1d219d02"),
            Guid.Parse("24ccd46c-0689-4471-94ab-164ab166bab7"),
            Guid.Parse("c464adef-37f4-43c5-885e-05af0f35e7c0"),
            Guid.Parse("e0e46f89-cbae-499f-9d9e-3e34440a60d9"),
            Guid.Parse("48717491-5db4-4a53-8d52-e17f7eb46db8"));

        var random = new Random();
        return _fixture.Build<Telemetry>()
            .With(x => x.EventSource, "IoT")
            .With(x => x.DeviceId, (IFixture f) => 
                f.Build<Guid>().FromFactory(deviceIdBuilder).Create())
            .With(x => x.Signals, _fixture.Build<Signal>()
                .With(x => x.Value,  random.Next(0, 100).ToString(CultureInfo.InvariantCulture))
                .CreateMany(2))
            .CreateMany(5);
    }
}