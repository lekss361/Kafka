using API;
using API.Extensions;
using API.HealthCheck;
using API.Model;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var kafkaConfig = new KafkaConfigModel();
builder.Configuration.Bind("services:kafka", kafkaConfig);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddKafkaServices(kafkaConfig);
    builder.Services.AddKafkaPublisher();
    builder.Services.AddKafkaConsumerList();
    builder.Services.AddHealthChecks().AddCheck<SampleHealthCheck>(nameof(SampleHealthCheck));
    builder.Services.AddHealthChecksUI().AddInMemoryStorage();




    var app = builder.Build();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHsts();
        app.UseKafkaFlowDashboard();
    }

    app.MapControllers();
    app.MapHealthChecks("/health", new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecksUI();

    app.Run();
