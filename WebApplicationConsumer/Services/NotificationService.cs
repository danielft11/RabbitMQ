using System;

namespace WebApplicationConsumer.Services
{
    public class NotificationService : INotificationService
    {
        public void NotifyUser(int fromId, int toId, string content)
        {
            // está vazio intencionamente. Apenas para elucidar que seria possível fazer algo após o recebimento da mensagem, por exemplo:
            // enviar e-mail, notificar o usuário com pushers, etc.
        }
    }
}
