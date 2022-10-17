using ProtoBuf;
using KafkaFlow;
using Newtonsoft.Json;

namespace API.Serialize;

public class CustomSerializer : ISerializer
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

    public async Task<string> JsonConvertSerilize(object data)
    {
       var result=  JsonConvert.SerializeObject(data, Formatting.Indented);
        return result;
    }
}