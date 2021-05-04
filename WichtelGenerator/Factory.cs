using SimpleInjector;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Lottery;
using WichtelGenerator.Core.Notification;

namespace WichtelGenerator.Core
{
    public class Factory
    {
        private readonly Container _container = new Container();

        public Factory()
        {
            _container.Register<INotificationManager, NotificationManager>(Lifestyle.Singleton);
            _container.Register<ILotteryService, LotteryService>(Lifestyle.Singleton);
            _container.Register<IConfigManager, ConfigManager>(Lifestyle.Singleton);
            _container.Register<INotificationConfig, NotificationConfig>(Lifestyle.Singleton);

            _container.Verify();
        }

        public ILotteryService GetLotteryService()
        {
            return _container.GetInstance<ILotteryService>();
        }

        public IConfigManager GetConfigManager()
        {
            return _container.GetInstance<IConfigManager>();
        }
    }
}