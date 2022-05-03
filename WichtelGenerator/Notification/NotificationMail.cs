using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TimMailLib;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Exeptions;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Notification
{
    public class NotificationMail : INotificationMail
    {
        public NotificationMail(IConfigManager configManager,
            IMailSender mailSender)
        {
            ConfigManager = configManager;
            //Enabled = configManager.ConfigModel.MailNotificationEnabled;
            MailSender = mailSender;
        }

        private IMailSender MailSender { get; }
        public bool Enabled { get; set; }

        public void SendRuffleResult()
        {
            SetSettings();
            if (!Enabled)
            {
                return;
            }

            var config = ConfigManager.ReadSettings();
            
            foreach (var oneSanta in config.SecretSantaModels)
            {
                var message =
                    $"Hallo {oneSanta.Name},\nDein Wichtel in der aktuellen Verlosung ist: {oneSanta.Choise}\nDies ist eine geheime Information"
                    + " niemand sonst weiss davon. Also versuche dies auch weiterhin geheim zu halten.";

                MailSender.MailSettings.EmpfaengerListe = new List<string> { oneSanta.MailAdress };
                try
                {
                    MailSender.SendMessage("Wichtel Generator - Verlosungsresultat", message);
                }
                catch (Exception e)
                {
                    throw new NotificationNotSendedException("Mail konnte nicht versendet werden", e);
                }
            }
        }

        public IConfigManager ConfigManager { get; set; }

        private void SetSettings()
        {
            Enabled = ConfigManager.ReadSettings().MailNotificationEnabled;
            if (!Enabled) return;

            var config = ConfigManager.ReadSettings();
            
            MailSender.MailSettings = new MailSettings
            {
                Absender = config.Absender, //--- First time read it, to fill up the Model
                //EmpfaengerListe = _container.GetInstance<IConfigManager>().Read().EmpfaengerListe, --> is not needed, because different mails are sent to different recipients.
                Passwort = config.Passwort,
                Port = config.Port,
                ServerName = config.ServerName,
                SslOn = config.SslOn,
                Username = config.Username
            };
        }
    }
}