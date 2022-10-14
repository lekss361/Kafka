namespace API.Model
{
    public class OutKafkaMessageModel
    {
        public OutKafkaMessageModel(long offset, string message)
        {
            Offset = offset;
            Message = message;
        }

        public long Offset { get; set; }
        public string Message { get; set; }
    }
}
