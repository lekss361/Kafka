
using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.Configuration;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.DependencyInjection.Extensions;
using API.Serialize;
using ProtoBuf.Meta;
using API.Model;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddKafkaConsumerService(this IServiceCollection services) => services.AddHostedService<KafkaConsumerHostedService>();

    public static IServiceCollection AddKafkaPublisher(this IServiceCollection services)
        => services.AddSingleton<IKafkaMessagePublisher, KafkaMessagePublisher>();

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
                                .AddMiddlewares(m => m.AddSerializer<ProtobufNetSerializer>())
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
                                        .AddSerializer<ProtobufNetSerializer>()
                                        .AddTypedHandlers(h => h.AddHandler<PrintDebugHandler>()
                                        )
                                )
                        )
                )
        );
        
        return services;
    }
}
