using System.Collections.Generic;
using System.Linq;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Notification
{
    internal class NotificationManager : INotificationManager
    {
        public NotificationManager(INotificationConfig notificationConfig)
        {
            NotificationConfig = notificationConfig;

            //Add new Notificationservices here!
            //NotificationServices.Add();
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