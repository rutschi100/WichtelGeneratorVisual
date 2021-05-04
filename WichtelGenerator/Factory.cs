﻿using SimpleInjector;
using TimMailLib;
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

            _container.Register<IMailSender>(() =>
                new MailSender(
                    new MailSettings
                    {
                        Absender = _container.GetInstance<IConfigManager>().Read().Absender,
                        //EmpfaengerListe = _container.GetInstance<IConfigManager>().Read().EmpfaengerListe, --> is not needed, because different mails are sent to different recipients.
                        Passwort = _container.GetInstance<IConfigManager>().Read().Passwort,
                        Port = _container.GetInstance<IConfigManager>().Read().Port,
                        ServerName = _container.GetInstance<IConfigManager>().Read().ServerName,
                        SslOn = _container.GetInstance<IConfigManager>().Read().SslOn,
                        Username = _container.GetInstance<IConfigManager>().Read().Username
                    }
                )
            );

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