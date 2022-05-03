using System;
using SimpleInjector;
using TimMailLib;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Lottery;
using WichtelGenerator.Core.Notification;

namespace WichtelGenerator.Core
{
    [Obsolete("This is not necessary if you use Mocking")]
    public class Factory
    {
        private readonly Container _container = new Container();

        public Factory()
        {
            _container.Register<INotificationManager, NotificationManager>(Lifestyle.Singleton);
            _container.Register<ILotteryService, LotteryService>(Lifestyle.Singleton);
            _container.Register<IConfigManager, ConfigManager>(Lifestyle.Singleton);
            _container.Register<INotificationMail, NotificationMail>(Lifestyle.Singleton);
            _container.RegisterSingleton<IMailSender>(() => new MailSender(new MailSettings()));

            /*
            _container.Register<IMailSender>(() =>
                new MailSender(
                    new MailSettings
                    {
                        Absender = _container.GetInstance<IConfigManager>().Read().Absender, //--- First time read it, to fill up the Model
                        //EmpfaengerListe = _container.GetInstance<IConfigManager>().Read().EmpfaengerListe, --> is not needed, because different mails are sent to different recipients.
                        Passwort = _container.GetInstance<IConfigManager>().ConfigModel.Passwort,
                        Port = _container.GetInstance<IConfigManager>().ConfigModel.Port,
                        ServerName = _container.GetInstance<IConfigManager>().ConfigModel.ServerName,
                        SslOn = _container.GetInstance<IConfigManager>().ConfigModel.SslOn,
                        Username = _container.GetInstance<IConfigManager>().ConfigModel.Username
                    }
                )
            );
            */

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