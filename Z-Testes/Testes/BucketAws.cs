using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;

namespace Testes
{
    public static class BucketAws
    {
        private const string bucketName = "checklistdelphi";
        private const string DownloadkeyName = "Checklist_Download_File";
        private const string UploadkeyName = "Checklist_Upload_File";

        private static readonly string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "teste");

        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private static IAmazonS3 s3Client;

        public static async Task UploadFileAsync()
        {
            s3Client = new AmazonS3Client(new BasicAWSCredentials
                    ("AKIAZRKH2SFLH5J54QJW", "TkTw8UPyUI589z04FPZugqxvpkpVqS0MWlRAUt7N"),
                bucketRegion);

            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DownloadkeyName),
                    bucketName,
                    DownloadkeyName);

                // // Option 1. Upload a file. The file name is used as the object key name.
                // await fileTransferUtility.UploadAsync(filePath, bucketName);
                // Console.WriteLine("Upload 1 completed");

                // Option 2. Specify object key name explicitly.
                await fileTransferUtility.UploadAsync(filePath, bucketName, UploadkeyName);
                Console.WriteLine("Upload 2 completed");
                //
                // // Option 3. Upload data from a type of System.IO.Stream.
                // using (var fileToUpload =
                //     new FileStream(filePath, FileMode.Open, FileAccess.Read))
                // {
                //     await fileTransferUtility.UploadAsync(fileToUpload,
                //         bucketName, up);
                // }
                //
                // Console.WriteLine("Upload 3 completed");
                //
                // // Option 4. Specify advanced settings.
                // var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                // {
                //     BucketName = bucketName,
                //     FilePath = filePath,
                //     StorageClass = S3StorageClass.StandardInfrequentAccess,
                //     PartSize = 6291456, // 6 MB.
                //     Key = keyName,
                //     CannedACL = S3CannedACL.PublicRead
                // };
                // fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                // fileTransferUtilityRequest.Metadata.Add("param2", "Value2");
                //
                // await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                // Console.WriteLine("Upload 4 completed");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}