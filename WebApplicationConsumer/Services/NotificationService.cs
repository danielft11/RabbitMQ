using System;
using System.IO;

namespace WebApplicationConsumer.Services
{
    public class NotificationService : INotificationService
    {
        public void NotifyUser(string exchangeType, int fromId, int toId, string content)
        {
            string filePath = @"D:\Desenvolvimento\Projetos Visual Studio\RabbitMQ\WebApplicationConsumer\ReceivedMessages\ReceivedMessages.txt";
            string lineToRecord = $"Exchange Type: {exchangeType} - FromId: {fromId} - ToId: {toId} - Content: {content} - Date: {DateTime.Now}";
            
            using var writer = new StreamWriter(path: filePath, append: true);
            writer.WriteLine(lineToRecord);
        }
    }
}
