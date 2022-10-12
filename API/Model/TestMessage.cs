using System.Runtime.Serialization;

namespace API.Model;

[DataContract]
public class TestMessage
{
    [DataMember(Order = 1)]
    public string Text { get; set; }
}
