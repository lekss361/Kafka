using KafkaFlow;
using KafkaFlow.TypedHandler;
using API.Serialize;
using API.Model;
using API.Services;

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
        const string producerName2 = "sample-topi";
        const string topicName = "sample-topic";
        
        services.AddKafkaConsumerService();
        services.AddKafka(
            kafka => kafka
                .UseConsoleLog()
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(kafkaConfigModel.Brokers)

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
                                .Topic(topicName)
                                .WithName(topicName)
                                .WithWorkersCount(2)
                                .WithBufferSize(1)
                                .WithGroupId(topicName)
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
                                .AddMiddlewares(m => m.AddSerializer<CustomSerializer>())
                        )
                        .AddProducer(
                            producerName2,
                            producer => producer
                                .DefaultTopic(producerName2)
                                .AddMiddlewares(m => m.AddSerializer<CustomSerializer>())
                        )
                        
                )
        );


        return services;
    }
}
