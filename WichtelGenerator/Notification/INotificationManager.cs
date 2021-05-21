using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WichtelGenerator.Core.Configuration;
using WichtelGenerator.Core.Models;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")] 

namespace WichtelGenerator.Core.Notification
{
    internal interface INotificationManager
    {
        void SendRaffle(IEnumerable<SecretSantaModel> secretSantas);
        public IConfigManager ConfigManager { get; set; }
    }
}