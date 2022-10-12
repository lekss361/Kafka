
using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.Configuration;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.DependencyInjection.Extensions;
using API.Serialize;
using ProtoBuf.Meta;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddKafkaConsumerService(this IServiceCollection services) => services.AddHostedService<KafkaConsumerHostedService>();

    public static IServiceCollection AddKafkaPublisher(this IServiceCollection services)
        => services.AddSingleton<IKafkaMessagePublisher, KafkaMessagePublisher>();

    public static IServiceCollection AddKafkaServices(this IServiceCollection services)
    {
        const string producerName = "sample-topic";
        const string topicName = "sample-topic";
        services.AddKafkaConsumerService();

        services.AddKafka(
            kafka => kafka
                .UseConsoleLog()
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(new[] {    "pr-sbpay-mq-1a1.nix.netlab.local:19091",
                                                "pr-sbpay-mq-1a2.nix.netlab.local:19092",
                                                "pr-sbpay-mq-1a3.nix.netlab.local:19093" })
                        .WithSecurityInformation(si =>
                        {
                            si.SecurityProtocol=SecurityProtocol.Ssl;
                            si.SslCaLocation = AppDomain.CurrentDomain.BaseDirectory + @"/Certificates/kafka-truststore.crt";
                            si.SslKeyLocation = AppDomain.CurrentDomain.BaseDirectory + @"/Certificates/kafka-client-keystore.key";
                            si.SslCertificateLocation = AppDomain.CurrentDomain.BaseDirectory + @"/Certificates/kafka-client-keystore.pem";
                            si.SslKeyPassword = "123456";
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
                                .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                                .WithGroupId(topicName)
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
