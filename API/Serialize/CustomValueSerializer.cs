using API.Model;
using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Serializer;
using System.Text.Json;
using ProtoBuf;

namespace API.Serialize;

public class ProtobufNetSerializer : ISerializer
{
    /// <inheritdoc/>
    public Task SerializeAsync(object message, Stream output, ISerializerContext context)
    {
       SerializeAsync(message,output,context);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<object> DeserializeAsync(Stream input, Type type, ISerializerContext context)
    {
        return Task.FromResult(DeserializeAsync(input, type, context).Result);
    }
}