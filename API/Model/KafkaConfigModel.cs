﻿using Confluent.Kafka;
using KafkaFlow.Configuration;

namespace API.Model;

public class KafkaConfigModel
{
    public string[] Brokers { get; set; } = Array.Empty<string>();
    public SecurityInformation SecurityInformation { get; set; } = new();
    public  ProducerConfig ProducerConfig { get; set; } = new();
    public ConsumerConfig ConsumerConfig { get; set; } = new();
}



