using System;
using System.Collections.Generic;
using System.Text;

namespace WichtelGenerator.Core.Exeptions
{
    public class NotificationNotSendedException : Exception
    {
        public NotificationNotSendedException() : base($"The message could not be sent")
        {
            
        }
    }
}
