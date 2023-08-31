namespace WebApplicationConsumer.Services
{
    public interface INotificationService
    {
        void NotifyUser(int fromId, int toId, string content);
    }
}
