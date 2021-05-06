using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Notification
{
    internal class NotificationManager : INotificationManager
    {
        public NotificationManager(INotificationConfig notificationConfig, INotificationMail mail)
        {
            NotificationConfig = notificationConfig;

            //Add new Notificationservices here!
            NotificationServices.Add(mail);
        }

        private List<INotification> NotificationServices { get; } = new List<INotification>();

        private INotificationConfig NotificationConfig { get; }

        public void SendRaffle(IEnumerable<SecretSantaModel> secretSantas)
        {
            if (!NotificationConfig.IamEnabled(this))
            {
                return;
            }

            foreach (var oneNotificationService in NotificationServices.Where(oneNotificationService =>
                oneNotificationService.Enabled))
            {
                oneNotificationService.SendRuffleResult();
            }
        }
    }
}