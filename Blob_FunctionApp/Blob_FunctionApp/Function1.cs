using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Blob_FunctionApp;

public class Function
{
    private readonly ILogger<Function> _logger;

    public Function(ILogger<Function> logger)
    {
        _logger = logger;
    }

    [Function(nameof(Function))]
    public async Task Run([BlobTrigger("training-container/{name}", Connection = "AzureWebJobsStorage")] Stream stream, string name)
    {
        using var blobStreamReader = new StreamReader(stream);
        var content = await blobStreamReader.ReadToEndAsync();
        _logger.LogInformation("C# Blob trigger function Processed blob\n Name: {name}", name/*, content*/);
    }
}