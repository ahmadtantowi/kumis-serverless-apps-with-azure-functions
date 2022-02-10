using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace AzFn.KuliahKamis
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;

        public HttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        [Function("HttpTrigger")]
        [OpenApiOperation(tags: "Test")]
        [OpenApiParameter("name", In = Microsoft.OpenApi.Models.ParameterLocation.Query, Required = true)]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, string name)
        {
            ArgumentNullException.ThrowIfNull(name);
            _logger.LogInformation("C# HTTP trigger function processed a request. hello {0}", name);

            var response = req.CreateResponse(HttpStatusCode.OK);
            // response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            // response.WriteString("Welcome to Azure Functions!");

            await response.WriteAsJsonAsync(new HelloWorld
            {
                Message = $"Hello {name}",
                Status = "I'm fine, thanks"
            });

            return response;
        }

        [Function("HttpTrigger2")]
        [OpenApiOperation(tags: "Real")]
        public async Task<HttpResponseData> Run2([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            // response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            // response.WriteString("Welcome to Azure Functions!");

            await response.WriteAsJsonAsync(new HelloWorld
            {
                Message = "Hello World",
                Status = "I'm Fine"
            });

            return response;
        }
    }

    public class HelloWorld
    {
        public string Message { get; set; }
        public string Status { get; set; }
    }
}
