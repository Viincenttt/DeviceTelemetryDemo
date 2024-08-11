using AutoFixture;
using DeviceTelemetry.Producer.BackgroundServices;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddUserSecrets(typeof(Program).Assembly)
    .AddEnvironmentVariables();

builder.Services.AddTransient<IFixture, Fixture>();
builder.Services.AddSingleton((s) => 
    new CosmosClient(builder.Configuration.GetConnectionString("CosmosDb")));

builder.Services.AddHostedService<TelemetryProducer>();

var host = builder.Build();
host.Run();