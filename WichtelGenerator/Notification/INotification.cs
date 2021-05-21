using System.Runtime.CompilerServices;
using WichtelGenerator.Core.Configuration;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Notification
{
    internal interface INotification
    {
        public bool Enabled { get; set; }
        public IConfigManager ConfigManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Sucsess</returns>
        public bool SendRuffleResult();
    }
}