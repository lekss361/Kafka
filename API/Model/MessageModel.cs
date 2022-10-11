using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace API.Model;

public class MessageModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
