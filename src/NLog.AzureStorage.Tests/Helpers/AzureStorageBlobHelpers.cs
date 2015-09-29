using System;
using System.Configuration;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace NLog.AzureStorage.Tests.Helpers
{
    public class AzureStorageBlobHelpers
    {
        private CloudBlobClient _cloudBlobClient;

        public AzureStorageBlobHelpers()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString);

            _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public void DeleteStorageContainer(string containerName)
        {
            var container = _cloudBlobClient.GetContainerReference(containerName);

            container.DeleteIfExists();
        }

        //public void DeleteAllAppendBlobs(string containerName)
        //{
        //    var container = _cloudBlobClient.GetContainerReference(containerName);

        //    foreach (var blobItem in container.ListBlobs())
        //    {
        //        var blob = container.GetAppendBlobReference(

        //        container.blob
        //    }

        //}

        public bool StorageContainerExists(string containerName)
        {
            var container = _cloudBlobClient.GetContainerReference(containerName);

            return (container.Exists());
        }

        public bool StorageAppendBlobExists(string containerName, string blobName)
        {
            var container = _cloudBlobClient.GetContainerReference(containerName);
            var appendBlob = container.GetAppendBlobReference(blobName);

            return (appendBlob.Exists());
        }

        public bool StorageAppendBlobWithTextExists(string containerName, string blobName, string text)
        {
            var container = _cloudBlobClient.GetContainerReference(containerName);
            var appendBlob = container.GetAppendBlobReference(blobName);

            var appendBlobContent = appendBlob.DownloadText();

            if (String.IsNullOrEmpty(appendBlobContent))
                return (false);

            if (appendBlobContent.Contains(text))
                return (true);

            return (false);
        }

        public static string FormatBlobName(string blobName)
        {
            return (String.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(blobName), DateTime.Now.ToString("dd-MM-yyyy"), Path.GetExtension(blobName)));
        }
    }
}
