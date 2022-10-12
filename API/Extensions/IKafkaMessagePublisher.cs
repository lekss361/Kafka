﻿using Confluent.Kafka;

namespace API.Extensions
{
    public interface IKafkaMessagePublisher
    {
        Task<DeliveryResult<byte[], byte[]>> PublishMessageAsync<T>(T message, string topic, IEnumerable<KeyValuePair<string, string>>? messageHeaders = null) where T : class;
    }
}