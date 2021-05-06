using System.Collections.Generic;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Configuration
{
    public class ConfigModel
    {
        #region Generel

        public List<SecretSantaModel> SecretSantaModels { get; set; }
        public bool NotificationsEnabled { get; set; }
        public bool MailNotificationEnabled { get; set; }

        #endregion

        #region MailSettings

        public IEnumerable<string> EmpfaengerListe { get; set; } = new List<string>();

        public int Port { get; set; }

        public string Absender { get; set; }

        public string ServerName { get; set; }

        public string Username { get; set; }

        public string Passwort { get; set; }

        public bool SslOn { get; set; }

        #endregion
    }
}