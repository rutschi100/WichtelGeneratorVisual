using System.Collections.Generic;
using WichtelGenerator.Core.Models;

namespace WichtelGenerator.Core.Notification
{
    internal interface INotificationManager
    {
        void SendRaffle(IEnumerable<SecretSantaModel> secretSantas);
    }
}