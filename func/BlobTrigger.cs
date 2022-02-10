using System;
using System.IO;
using func.Abstractions;
using func.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzFn.KuliahKamis
{
    public class BlobTrigger
    {
        private readonly ILogger _logger;
        private readonly INotificationService _notifService;

        public BlobTrigger(ILoggerFactory loggerFactory, INotificationService notifService)
        {
            _logger = loggerFactory.CreateLogger<BlobTrigger>();
            _notifService = notifService;
        }

        [Function("BlobTrigger")]
        public void Run([BlobTrigger("sample/{name}", Connection = "AzureWebJobsStorage")] string myBlob, string name)
        {
            _logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} \n Data: {myBlob}");
            _notifService.SendEmail("ahmad@radyalabs.com");
        }
    }
}
