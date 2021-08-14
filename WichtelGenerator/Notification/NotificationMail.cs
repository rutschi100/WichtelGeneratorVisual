using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TimMailLib;
using WichtelGenerator.Core.Configuration;

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

        public bool SendRuffleResult()
        {
            SetSettings();
            if (!Enabled)
            {
                return false;
            }

            foreach (var oneSanta in ConfigManager.ReadSettings().SecretSantaModels)
            {
                var message =
                    $"Hallo {oneSanta.Name},\nDein Wichtel in der aktuellen Verlosung ist: {oneSanta.Choise}\nDies ist eine geheime Information" +
                    " niemand sonst weiss davon. Also versuche dies auch weiterhin geheim zu halten.";

                MailSender.MailSettings.EmpfaengerListe = new List<string>
                {
                    oneSanta.MailAdress
                };
                try
                {
                    MailSender.SendMessage("Wichtel Generator - Verlosungsresultat", message);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public IConfigManager ConfigManager { get; set; }

        private void SetSettings()
        {
            Enabled = ConfigManager.ReadSettings().MailNotificationEnabled;
            if (!Enabled) return;
            MailSender.MailSettings = new MailSettings
            {
                Absender = ConfigManager.ReadSettings().Absender, //--- First time read it, to fill up the Model
                //EmpfaengerListe = _container.GetInstance<IConfigManager>().Read().EmpfaengerListe, --> is not needed, because different mails are sent to different recipients.
                Passwort = ConfigManager.ConfigModel.Passwort,
                Port = ConfigManager.ConfigModel.Port,
                ServerName = ConfigManager.ConfigModel.ServerName,
                SslOn = ConfigManager.ConfigModel.SslOn,
                Username = ConfigManager.ConfigModel.Username
            };
        }
    }
}