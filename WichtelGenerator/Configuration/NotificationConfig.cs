using WichtelGenerator.Core.Exeptions;

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
            var configs = ConfigManager.Read();

            switch (T.GetType().Name)
            {
                case "NotificationManager":
                {
                    return configs.NotificationsEnabled;
                }
                default:
                {
                    throw new ConfigUnknownTypeExeption(T.GetType().Name);
                }
            }
        }
    }
}