using System.Collections.Generic;
using TimMailLib;
using WichtelGenerator.Core.Configuration;

namespace WichtelGenerator.Core.Notification
{
    internal class NotificationMail : INotification
    {
        public NotificationMail(IConfigManager configManager, INotificationConfig notificationConfig,
            IMailSender mailSender)
        {
            ConfigManager = configManager;
            NotificationConfig = notificationConfig;
            MailSender = mailSender;

            Enabled = notificationConfig.IamEnabled(this);
        }

        private IMailSender MailSender { get; }
        public bool Enabled { get; set; }
        public INotificationConfig NotificationConfig { get; set; }

        public void SendRuffleResult()
        {
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