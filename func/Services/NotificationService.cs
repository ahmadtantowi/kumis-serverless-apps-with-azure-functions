using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using func.Abstractions;
using Microsoft.Extensions.Logging;

namespace func.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendEmail(string email)
        {
            _logger.LogInformation("Email terkirim untuk {0}", email);

            return Task.CompletedTask;
        }

        public Task SendPushNotif()
        {
            throw new NotImplementedException();
        }
    }
}