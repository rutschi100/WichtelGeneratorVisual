using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Exeptions;
using WichtelGenerator.Core.Models;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace WichtelGenerator.Core.Notification
{
    public class NotificationManager : INotificationManager
    {
        public NotificationManager(INotificationMail mail, IConfigManager configManager)
        {
            ConfigManager = configManager;
            //Add new Notificationservices here!
            NotificationServices.Add(mail);
        }

        private List<INotification> NotificationServices { get; } = new List<INotification>();
        public IConfigManager ConfigManager { get; set; }

        public void SendRaffle(IEnumerable<SecretSantaModel> secretSantas)
        {
            if (!ConfigManager.ReadSettings().NotificationsEnabled)
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