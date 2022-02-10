using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using func.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace func.Middlewares
{
    public class ExceptionMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                
                var request = context.GetHttpRequestData();
                if (request != null) // ensure trigger is HttpTrigger
                {
                    var response = request.CreateResponse(HttpStatusCode.InternalServerError);
                    response.WriteString(ex.Message);
                    context.InvokeResult(response);

                    return;
                }

                throw;
            }
        }
    }
}