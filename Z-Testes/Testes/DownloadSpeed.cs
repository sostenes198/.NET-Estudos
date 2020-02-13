using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace Testes
{
    public static class DownloadSpeed
    {
        private const string bucketName = "checklistdelphi";
        private const string DownloadkeyName = "Checklist_Download_File";
        private const string UploadkeyName = "Checklist_Upload_File";
        private const int AmountDownload = 100;

        private static IAmazonS3 s3Client = new AmazonS3Client(
            "AKIAZRKH2SFLH5J54QJW", "TkTw8UPyUI589z04FPZugqxvpkpVqS0MWlRAUt7N",
            new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1,
                CacheHttpClient = false
            });

        private static readonly TransferUtility FileTransferUtility =
            new TransferUtility(s3Client);

        public static async Task Speed()
        {
            var pathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), UploadkeyName);
            var speedDonwload = new List<double>();
            for (int i = 0; i < AmountDownload; i++)
            {
                var sw = Stopwatch.StartNew();
                await FileTransferUtility.DownloadAsync(pathFile, bucketName, DownloadkeyName);
                sw.Stop();
                
                var fileInfo = new FileInfo(pathFile); 
                speedDonwload.Add(fileInfo.Length / sw.Elapsed.TotalSeconds);
            }
        }

        public static async Task Speed1()
        {
            var pathFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), UploadkeyName);
            var speedDonwload = new List<double>();
            for (int i = 0; i < AmountDownload; i++)
            {
                var dt1 = DateTime.Now;
                var result = await FileTransferUtility.S3Client.GetObjectAsync(bucketName, DownloadkeyName);
                var dt2 = DateTime.Now;
                speedDonwload.Add(Math.Round(((double) result.ResponseStream.Length / 1024) / (dt2 - dt1).TotalSeconds, 2));
            }
        }
    }
}