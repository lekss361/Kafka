using API.Extensions;
using API.HealthCheck;
using API.Model;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var kafkaConfig = new KafkaConfigModel();
builder.Configuration.Bind("services:kafka", kafkaConfig);

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
}

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

app.Run();
