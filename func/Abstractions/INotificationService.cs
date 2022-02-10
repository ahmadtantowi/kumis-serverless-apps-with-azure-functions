using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace func.Abstractions
{
    public interface INotificationService
    {
        Task SendEmail(string email);
        Task SendPushNotif();
    }
}