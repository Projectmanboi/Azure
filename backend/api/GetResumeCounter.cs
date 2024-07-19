using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.Cosmos;

namespace Company.Function
{
    public static class GetResumeCounter
    {
        [FunctionName("GetResumeCounter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"AzureResume", containerName: "Counter", Connection = "AzureResumeConnectionString", Id = "1", PartitionKey ="1")] Counter counter,
            [CosmosDB(databaseName:"AzureResume", containerName: "Counter", Connection = "AzureResumeConnectionString", PartitionKey = "1")] IAsyncCollector<Counter> counters,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

string name = req.Query["name"];
string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
dynamic data = JsonConvert.DeserializeObject(requestBody);
name = name ?? data?.name;

if (counter == null)
{
    log.LogWarning("Counter document not found.");
    return new NotFoundResult();
}

log.LogInformation($"Current counter value: {counter.Count}");
counter.Count++;
log.LogInformation($"Current counter value: {counter.Count}");

await counters.AddAsync(counter);

var response = new { id = counter.Id, count = counter.Count };

return new OkObjectResult(response);
            }
        }
    }
      public class Counter
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }

