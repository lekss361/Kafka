using API;
using KafkaFlow.Configuration;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var topicOptions = new KafkaTopicOptions();
builder.Configuration.Bind("services:kafka", topicOptions);
var kafkaOptions = new ProducerOptions();
builder.Configuration.Bind("services:kafka", kafkaOptions);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
