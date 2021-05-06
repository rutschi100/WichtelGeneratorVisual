using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WichtelGenerator.Core.Test")]

namespace WichtelGenerator.Core.Configuration
{
    internal interface INotificationConfig
    {
        bool IamEnabled(object T);
    }
}