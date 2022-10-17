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

    public static IServiceCollection AddKafkaServices(this IServiceCollection services, KafkaConfigModel kafkaConfigModel)
    {
        const string producerName = "sample-topic";
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
                            si.SecurityProtocol=kafkaConfigModel.SecurityInformation.SecurityProtocol;
                            si.SslCaLocation = kafkaConfigModel.SecurityInformation.SslCaLocation;
                            si.SslKeyLocation = kafkaConfigModel.SecurityInformation.SslKeyLocation;
                            si.SslCertificateLocation = kafkaConfigModel.SecurityInformation.SslCertificateLocation;
                            si.SslKeyPassword = kafkaConfigModel.SecurityInformation.SslKeyPassword;
                        })
                        .AddProducer(
                            producerName,
                            producer => producer
                                .DefaultTopic(topicName)
                                .AddMiddlewares(m => m.AddSerializer<CustomSerializer>())
                        )
                        .AddConsumer(
                            consumer => consumer
                                .Topic(topicName)
                                .WithConsumerConfig(kafkaConfigModel.ConsumerConfig)
                                .WithName(topicName)
                                .WithBufferSize(100)
                                .WithWorkersCount(20)
                                .AddMiddlewares(
                                    middlewares => middlewares
                                        .AddSerializer<CustomSerializer>()
                                        .AddTypedHandlers(h => h.AddHandler<ConsumMessagesHandler>()
                                        )
                                )
                        )
                )
        );
        
        return services;
    }
}
