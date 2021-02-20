using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Estudos.AzureFunctions
{
    public static class FanInFanOut
    {
        [FunctionName("E2_BackupSiteContent")]
        public static async Task<long> Run([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var rootDirectory = Directory.GetParent(typeof(FanInFanOut).Assembly.Location).FullName;

            var files = await context.CallActivityAsync<string[]>(
                "E2_GetFileList",
                rootDirectory);

            var tasks = new Task<long>[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                tasks[i] = context.CallActivityAsync<long>("E2_CopyFileToBlob",
                    files[i]);
            }

            await Task.WhenAll(tasks);
            var totalBytes = tasks.Sum(t => t.GetAwaiter().GetResult());
            return totalBytes;
        }

        [FunctionName("E2_GetFileList")]
        public static string[] GetFileList([ActivityTrigger] string rootDirectory, ILogger logger)
        {
            logger.LogInformation($"Searching for files under ´{rootDirectory}´...");
            var files = Directory.GetFiles(rootDirectory, "*", SearchOption.AllDirectories);
            logger.LogInformation($"Found {files.Length} files(s) under {rootDirectory}.");
            return files;
        }

        [FunctionName("E2_CopyFileToBlob")]
        public static long CopyFileToBlob([ActivityTrigger] string filePath, Binder binder, ILogger logger)
        {
            long byteCount = new FileInfo(filePath).Length;

            var blobPath = filePath
                .Substring(Path.GetPathRoot(filePath).Length)
                .Replace('\\', '/');
            var outPutLocation = $"backups/{blobPath}";

            logger.LogInformation($"Copy ´{filePath}´ to ´{outPutLocation}´. Total bytes = {byteCount}.");

            // await using var source = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            // await using var destination = await binder.BindAsync<CloudBlobStream>(new BlobAttribute(outPutLocation, FileAccess.Write));
            // await source.CopyToAsync(destination);
            return byteCount;
        }
    }
}