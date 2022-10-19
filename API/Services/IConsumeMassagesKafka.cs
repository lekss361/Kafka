namespace API.Extensions
{
    public interface IConsumeMassagesKafka
    {
        Task<string> PrintLastMessages(int count);
    }
}