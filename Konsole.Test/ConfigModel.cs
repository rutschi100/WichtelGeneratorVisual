using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Konsole.Test;

namespace WichtelGenerator.Core.Configuration
{
    public class ConfigModel
    {
        [Key]
        public int ID { get; set; }
        
        #region AppSettings

        public int FontSize { get; set; } = 16;

        #endregion

        #region Generel

        public List<SecretSantaModel> SecretSantaModels { get; set; }
        public bool NotificationsEnabled { get; set; }
        public bool MailNotificationEnabled { get; set; }

        #endregion

        #region MailSettings

        public IEnumerable<MailAdressModel> EmpfaengerListe { get; set; } = new List<MailAdressModel>();

        public int Port { get; set; }

        public string Absender { get; set; }

        public string ServerName { get; set; }

        public string Username { get; set; }

        public string Passwort { get; set; }

        public bool SslOn { get; set; }

        #endregion
    }

    public class MailAdressModel
    {
        [Key]
        public int ID { get; set; }
        
        public string Mail { get; set; }
    }
}