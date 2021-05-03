namespace WichtelGenerator.Core.Configuration
{
    internal class NotificationConfig : INotificationConfig
    {
        public NotificationConfig(IConfigManager configManager)
        {
            ConfigManager = configManager;
        }

        private IConfigManager ConfigManager { get; }

        public bool IamEnabled(object T)
        {
            //TODO: Implementieren
            return false;
        }
    }
}