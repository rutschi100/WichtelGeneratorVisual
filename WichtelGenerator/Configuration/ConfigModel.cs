using System.Collections.Generic;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Configuration
{
    public class ConfigModel
    {
        public List<SecretSantaModel> SecretSantaModels { get; set; }
        public bool NotificationsEnabled { get; set; }
        public bool MailNotificationEnabled { get; set; }
    }
}