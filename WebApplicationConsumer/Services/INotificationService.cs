namespace WebApplicationConsumer.Services
{
    public interface INotificationService
    {
        void NotifyUser(string exchangeType, int fromId, int toId, string content);
    }
}
