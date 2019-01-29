using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace Estudos.Azure.AccountStorage
{
    class Program
    {
        private static readonly string _filePdf = "Teste.pdf";
        private static readonly string _filePng = "Teste.png";
        private static readonly string _fileTxt = "Teste.txt";
        private static readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=estudossoso;AccountKey=P9rfVKd0dBHVvAXJJm7OJCqWmEuABhc7NNCXYxDgVdbiFR10BTXUVOP1crE1hZ2p77CFLsvTG1AEMqRxjwAhqw==;EndpointSuffix=core.windows.net";
        private static readonly string _containerName = "estudos-soso-test";

        private static readonly BlobServiceClient _blobServiceClient = new BlobServiceClient(_connectionString);

        static async Task Main(string[] args)
        {
        }

        private async Task CreateBlobContainerAndSaveBlobs()
        {
            var containerClient = await _blobServiceClient.CreateBlobContainerAsync(_containerName);

            await SaveBlob(containerClient.Value, _filePdf);
            await SaveBlob(containerClient.Value, _filePng);
            await SaveBlob(containerClient.Value, _filePdf);
        }

        private static async Task SaveBlob(BlobContainerClient blobContainerClient, string fileName)
        {
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            await using FileStream uploadFileStream = File.OpenRead(fileName);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();
        }

        private async Task ListBlobs()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                Console.WriteLine(blobItem.Name);
            }
        }

        private static async Task DownloadBlobs()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await DownloadBlob(blobContainerClient, _filePdf);
            await DownloadBlob(blobContainerClient, _filePng);
            await DownloadBlob(blobContainerClient, _fileTxt);
        }

        private static async Task DownloadBlob(BlobContainerClient blobContainerClient, string fileName)
        {
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            var download = await blobClient.DownloadAsync();
            using (var downloadFileStream = File.OpenWrite("1" + fileName))
            {
                await download.Value.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }
        }

        private static async Task DeleteBlobs()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await DeleteBlob(blobContainerClient, _filePdf);
            await DeleteBlob(blobContainerClient, _filePng);
            await DeleteBlob(blobContainerClient, _fileTxt);
        }

        private static async Task DeleteBlob(BlobContainerClient blobContainerClient, string fileName)
        {
            var blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        private static async Task DeleteContainer()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await blobContainerClient.DeleteAsync();
        }
    }
}