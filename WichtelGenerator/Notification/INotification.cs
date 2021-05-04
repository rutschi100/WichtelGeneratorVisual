using WichtelGenerator.Core.Configuration;

namespace WichtelGenerator.Core.Notification
{
    internal interface INotification
    {
        public bool Enabled { get; set; }
        public INotificationConfig NotificationConfig { get; set; }
        public IConfigManager ConfigManager { get; set; }
        public void SendRuffleResult();
    }
}