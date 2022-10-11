using API.Model;
using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace API.Serialize;

public static class CustomValueSerializer
{
    public static byte[] Serialize(MessageModel data)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
    }
}
