using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace StravaDataCollection
{
    public class HttpDataCreation
    {
        private readonly ILogger<HttpDataCreation> _logger;

        public HttpDataCreation(ILogger<HttpDataCreation> logger)
        {
            _logger = logger;
        }

        [Function("HttpDataCreation")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions: HttpDataCreation");
        }
    }
}
