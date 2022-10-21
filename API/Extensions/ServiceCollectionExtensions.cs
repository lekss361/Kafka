using KafkaFlow;
using KafkaFlow.TypedHandler;
using API.Serialize;
using API.Model;
using API.Services;
using KafkaFlow.Admin;
using Confluent.Kafka;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKafkaConsumerService(this IServiceCollection services) => services.AddHostedService<KafkaConsumerHostedService>();

    public static IServiceCollection AddKafkaPublisher(this IServiceCollection services)
        => services.AddSingleton<IMessagePublisherService, MessagePublisherService>();

    public static IServiceCollection AddKafkaConsumerList(this IServiceCollection services)
        => services.AddScoped<IConsumeMassagesKafka, ConsumeMassagesKafka>();

    public static void RegisterLogger(this IServiceCollection service, IConfiguration config)
    {
        service.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
        service.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Debug);
        });
    }

    public static IServiceCollection AddKafkaServices(this IServiceCollection services, KafkaConfigModel kafkaConfigModel)
    {
        
        const string producerName = "sample-topic";
        const string producerName2 = "debug";
        const string topicName = "sample-topiz";
        ConsumerConfig keyValuePairs = new(kafkaConfigModel.ConsumerConfig);
        keyValuePairs.AllowAutoCreateTopics = true;
        foreach (var item in kafkaConfigModel.ProducerConfigs)
        {
            keyValuePairs.Set(item.Key, item.Value);
        }
        services.AddKafkaConsumerService();
        services.AddKafka(
            kafka => kafka
                .UseConsoleLog()
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(kafkaConfigModel.Brokers)
                        .EnableTelemetry("kafka-flow.admin")

                        .WithSecurityInformation(si =>
                        {
                            si.SecurityProtocol = kafkaConfigModel.SecurityInformation.SecurityProtocol;
                            si.SslCaLocation = kafkaConfigModel.SecurityInformation.SslCaLocation;
                            si.SslKeyLocation = kafkaConfigModel.SecurityInformation.SslKeyLocation;
                            si.SslCertificateLocation = kafkaConfigModel.SecurityInformation.SslCertificateLocation;
                            si.SslKeyPassword = kafkaConfigModel.SecurityInformation.SslKeyPassword;
                        })
                        .AddConsumer(
                            consumer => consumer
                                .WithConsumerConfig(keyValuePairs)
                                .Topic(topicName)
                                .WithName(topicName)
                                .WithWorkersCount(20)
                                .WithBufferSize(100)
                                .AddMiddlewares(
                                    middlewares => middlewares
                                        .AddSerializer<CustomSerializer>()
                                        .AddTypedHandlers(h => h.AddHandler<ConsumeMessagesHandler>()
                                        )
                                )
                        )
                        .AddConsumer(
                            consumer => consumer
                                .WithConsumerConfig(keyValuePairs)
                                .Topic(producerName2)
                                .WithName(producerName2)
                                .WithWorkersCount(20)
                                .WithBufferSize(100)
                                .AddMiddlewares(
                                    middlewares => middlewares
                                        .AddSerializer<CustomSerializer>()
                                        .AddTypedHandlers(h => h.AddHandler<ConsumeMessagesHandler>()
                                        )
                                )
                        )
                        .AddProducer(
                            producerName,
                            producer => producer
                                .DefaultTopic(producerName)
                                .AddMiddlewares(m => m.AddSerializer<CustomSerializer>().Add<ErrorHandlingMiddleware>())
                        )
                        .AddProducer(
                            producerName2,
                            producer => producer
                                //.WithProducerConfig(keyValuePairs)
                                .DefaultTopic(producerName2)
                                .AddMiddlewares(middlewareBuilder => middlewareBuilder.Add<ErrorHandlingMiddleware>())
                                .AddMiddlewares(m => m.AddSerializer<CustomSerializer>())

                                )
                        
                )
        );


        return services;
    }
}
