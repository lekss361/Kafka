using API;
using API.Extensions;
using API.Model;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);
var kafkaConfig = new KafkaConfigModel();
builder.Configuration.Bind("services:kafka", kafkaConfig);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddKafkaServices(kafkaConfig);
builder.Services.AddKafkaPublisher();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
