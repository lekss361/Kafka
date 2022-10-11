using API.Model;
using Confluent.Kafka;

namespace API.Extensions;

public class ReaderAppsetting
{
    private KafkaConfigModel kafkaConfiguration = new KafkaConfigModel();

    public KafkaConfigModel GetProducerConfig()
    {
        var config = ReadAppSettings();
        Dictionary<string, string> GetConfig = config.GetSection("services:kafka:producerConfig").Get<Dictionary<string, string>>();
        string baseDirectory = config.GetSection("services:kafka")["BaseDirectory"];
        kafkaConfiguration.SecurityInformation.SecurityProtocol = (KafkaFlow.Configuration.SecurityProtocol)Enum.Parse(typeof(KafkaFlow.Configuration.SecurityProtocol), GetConfig["security.protocol"], true);
        kafkaConfiguration.SecurityInformation.SslCaLocation = baseDirectory + GetConfig["ssl.ca.location"];
        kafkaConfiguration.SecurityInformation.SslCertificateLocation = baseDirectory + GetConfig["ssl.certificate.location"];
        kafkaConfiguration.SecurityInformation.SslKeyLocation = baseDirectory + GetConfig["ssl.key.location"];
        kafkaConfiguration.SecurityInformation.SslKeyPassword = GetConfig["ssl.key.password"];
        return kafkaConfiguration;
    }

    public KafkaConfigModel GetConsumerConfig()
    {
        var config = ReadAppSettings();
        Dictionary<string, string> GetConfig = config.GetSection("services:kafka:consumerConfig").Get<Dictionary<string, string>>();
        string baseDirectory = config.GetSection("services:kafka")["BaseDirectory"];
        kafkaConfiguration.ConsumerConfig.BootstrapServers = string.Join(",", kafkaConfiguration.Brokers);
        kafkaConfiguration.ConsumerConfig.SecurityProtocol = (SecurityProtocol?)Enum.Parse(typeof(SecurityProtocol), GetConfig["security.protocol"], true);
        kafkaConfiguration.ConsumerConfig.SslCaLocation = baseDirectory + GetConfig["ssl.ca.location"];
        kafkaConfiguration.ConsumerConfig.SslCertificateLocation = baseDirectory + GetConfig["ssl.certificate.location"];
        kafkaConfiguration.ConsumerConfig.SslKeyLocation = baseDirectory + GetConfig["ssl.key.location"];
        kafkaConfiguration.ConsumerConfig.SslKeyPassword = GetConfig["ssl.key.password"];
        kafkaConfiguration.ConsumerConfig.EnableAutoCommit = Convert.ToBoolean(GetConfig["EnableAutoCommit"]);
        kafkaConfiguration.ConsumerConfig.AutoOffsetReset = (AutoOffsetReset)Enum.Parse(typeof(AutoOffsetReset), GetConfig["auto.offset.reset"], true);
        kafkaConfiguration.ConsumerConfig.GroupId = GetConfig["GroupId"];
        return kafkaConfiguration;
    }


    private IConfigurationRoot ReadAppSettings()
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"appsettings.json");
        IConfigurationRoot config = configuration.Build();
        string[] brokers = config.GetSection("services:kafka:Brokers").Get<string[]>();
        kafkaConfiguration.Brokers = brokers;
        return config;
    }


}
