using API.Model;
using System.Text.Json;
using ProtoBuf;
using KafkaFlow;
using Confluent.Kafka;
using ProtoBuf;

namespace API.Serialize;

public class ProtobufNetSerializer : ISerializer
{
    /// <inheritdoc/>
    public Task SerializeAsync(object message, Stream output, ISerializerContext context)
    {
        Serializer.Serialize(output, message);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<object> DeserializeAsync(Stream input, Type type, ISerializerContext context)
    {
        return Task.FromResult(Serializer.Deserialize(type, input));
    }
}