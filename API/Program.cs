using API.Extensions;
using API.HealthCheck;
using API.Model;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);
var kafkaConfig = new KafkaConfigModel();
builder.Configuration.Bind("services:kafka", kafkaConfig);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{

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
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.MapControllers();
    app.MapHealthChecks("/health", new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecksUI();

    app.Run();
}
catch(Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}