using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using func.Services;
using func.Abstractions;
using func.Middlewares;

namespace AzFn.KuliahKamis
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(worker => worker.UseMiddleware<ExceptionMiddleware>())
                .ConfigureServices(services => services.AddSingleton<INotificationService, NotificationService>())
                .ConfigureOpenApi()
                .Build();

            host.Run();
        }
    }
}