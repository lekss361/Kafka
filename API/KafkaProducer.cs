using API.Extensions;
using API.Serialize;
using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Producers;
using System.Diagnostics;

namespace API;

public class KafkaProducer
{

    public async Task AddMessageProducer(string topicName, Model.MessageModel message)
    {


        var serviceCollection = new ServiceCollection();

        AddKafka(topicName, serviceCollection);

        var provider = serviceCollection.BuildServiceProvider();

        var producerAccessor = provider.GetRequiredService<IProducerAccessor>();

        var producer = producerAccessor.GetProducer(topicName);


        try
        {
            await producer.ProduceAsync(Guid.NewGuid().ToString(), CustomValueSerializer.Serialize(message));
        }
        catch (ProduceException<string, string> e)
        {
            Debug.WriteLine($"failed to deliver message: {e.Message} [{e.Error.Code}]");
        }
    }


    private static void AddKafka(string topicName, ServiceCollection services)
    {
        ReaderAppsetting readerAppsetting = new ReaderAppsetting();
        var kafkaConfig = readerAppsetting.GetProducerConfig();
        var ss = kafkaConfig.SecurityInformation;
        services.AddKafka(kafka =>
        {
            kafka
                .UseConsoleLog()
                .AddCluster(cluster =>
                {
                    cluster
                        .WithBrokers(kafkaConfig.Brokers)

                            .WithSecurityInformation(si =>
                            {
                                si.SecurityProtocol = kafkaConfig.SecurityInformation.SecurityProtocol;
                                si.SslCaLocation = kafkaConfig.SecurityInformation.SslCaLocation;
                                si.SslKeyLocation = kafkaConfig.SecurityInformation.SslKeyLocation;
                                si.SslKeyPassword = kafkaConfig.SecurityInformation.SslKeyPassword;
                                si.SslCertificateLocation = kafkaConfig.SecurityInformation.SslCertificateLocation;
                            })
                            .AddProducer(topicName, producer =>
                            {
                                producer.DefaultTopic(topicName);
                            });
                });
        });
    }
}
