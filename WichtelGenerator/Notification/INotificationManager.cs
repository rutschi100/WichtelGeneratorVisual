using System.Collections.Generic;
using System.Runtime.CompilerServices;
using WichtelGenerator.Core.Models;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Notification
{
    internal interface INotificationManager
    {
        void SendRaffle(IEnumerable<SecretSantaModel> secretSantas);
    }
}