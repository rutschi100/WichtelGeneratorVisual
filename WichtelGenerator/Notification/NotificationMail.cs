using System.Collections.Generic;
using TimMailLib;
using WichtelGenerator.Core.Configuration;

namespace WichtelGenerator.Core.Notification
{
    internal class NotificationMail : INotificationMail
    {
        public NotificationMail(IConfigManager configManager, INotificationConfig notificationConfig)
        {
            ConfigManager = configManager;
            NotificationConfig = notificationConfig;
            Enabled = notificationConfig.IamEnabled(this);
        }

        private IMailSender MailSender { get; set; }
        public bool Enabled { get; set; }
        public INotificationConfig NotificationConfig { get; set; }

        private void SetSettings()
        {
            Enabled = NotificationConfig.IamEnabled(this);
            if (Enabled)
            {
                MailSender = new MailSender(new MailSettings()
                {
                    Absender = ConfigManager.Read().Absender, //--- First time read it, to fill up the Model
                    //EmpfaengerListe = _container.GetInstance<IConfigManager>().Read().EmpfaengerListe, --> is not needed, because different mails are sent to different recipients.
                    Passwort = ConfigManager.ConfigModel.Passwort,
                    Port = ConfigManager.ConfigModel.Port,
                    ServerName = ConfigManager.ConfigModel.ServerName,
                    SslOn = ConfigManager.ConfigModel.SslOn,
                    Username = ConfigManager.ConfigModel.Username
                });
            }
        }

        public void SendRuffleResult()
        {
            SetSettings();
            if (!Enabled)
            {
                return;
            }
            foreach (var oneSanta in ConfigManager.Read().SecretSantaModels)
            {
                var message =
                    $"Hallo {oneSanta.Name},\nDein Wichtel in der aktuellen Verlosung ist: {oneSanta.Choise}\nDies ist eine geheime Information" +
                    " niemand sonst weiss davon. Also versuche dies auch weiterhin geheim zu halten.";

                MailSender.MailSettings.EmpfaengerListe = new List<string>
                {
                    oneSanta.MailAdress
                };

                MailSender.SendMessage("Wichtel Generator - Verlosungsresultat", message);
            }
        }

        public IConfigManager ConfigManager { get; set; }
    }
}