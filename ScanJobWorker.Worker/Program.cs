using ScanJobWorker.Application.Features.Scans;
using ScanJobWorker.Infrastructure;
using ScanJobWorker.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton<ScanConsumer>();
builder.Services.AddHostedService<Worker>();

builder.Build().Run();