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
            _container.Register<INotificationManager, NotificationManager>();
            _container.Register<ILotteryService, LotteryService>();
            //_container.Register<IConfigManager, ConfigManager>();
            _container.Register<INotificationConfig, NotificationConfig>();

            _container.Verify();
        }

        public ILotteryService GetLotteryService()
        {
            return _container.GetInstance<ILotteryService>();
        }
    }
}