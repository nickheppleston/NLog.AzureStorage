using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;

namespace NLog.AzureStorage
{
    public class AzureBlobStorageProxy
    {
        private CloudBlobContainer _blobContainer;
        private CloudAppendBlob _appendBlob;

        public string StorageConnectionString { get; private set; }

        public string StorageContainerName { get; private set; }

        public string StorageBlobName { get; private set; }

        public bool EnableDebug { get; private set; }

        public AzureBlobStorageProxy(string storageConnectionString, string storageContainerName, string storageBlobName, bool enableDebug)
        {
            StorageConnectionString = storageConnectionString;
            StorageContainerName = storageContainerName;
            StorageBlobName = storageBlobName;
            EnableDebug = enableDebug;

            WriteDebugInfo(String.Format("NLog.AzureStorage - StorageConnectionString: {0}", StorageConnectionString));
            WriteDebugInfo(String.Format("NLog.AzureStorage - StorageContainerName:    {0}", StorageContainerName));
            WriteDebugInfo(String.Format("NLog.AzureStorage - StorageBlobName:         {0}", StorageBlobName));
            WriteDebugInfo(String.Format("NLog.AzureStorage - EnableDebug:             {0}", EnableDebug));
        }

        public void WriteTextToBlob(string text)
        {
            var formattedStorageContainerName = FormatContainerName(StorageContainerName);
            var formattedStorageBlobName = FormatBlobName(StorageBlobName);

            if (!string.IsNullOrEmpty(text))
            {
                InstanciateStorageContainer(formattedStorageContainerName);
                InstanciateStorageBlob(formattedStorageContainerName, formattedStorageBlobName);

                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
                {
                    WriteDebugInfo(String.Format("NLog.AzureStorage - {0}/{1} - Successfully wrote bytes: {2}", formattedStorageContainerName, formattedStorageBlobName, memoryStream.Length));

                    _appendBlob.AppendBlock(memoryStream);
                }
            }
        }

        internal static string FormatBlobName(string blobName)
        {
            return (String.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(blobName), DateTime.Now.ToString("dd-MM-yyyy"), Path.GetExtension(blobName)));
        }

        internal static string FormatContainerName(string containerName)
        {
            // Container names may only contain lowercase letters, numbers, and hyphens and must start with a letter or number.
            // The name cannot contain two consecutive hyphens. The name must also be between 3 and 63 characters long.

            var containerNameInvalidCharactersRemoved = Regex.Replace(containerName, "[^abcdefghijklmnopqrstuvwxyz0123456789-]", String.Empty, RegexOptions.IgnoreCase);

            return (containerNameInvalidCharactersRemoved.ToLower());
        }

        private void InstanciateStorageContainer(string formattedStorageContainerName)
        {
            if ((_blobContainer == null) || (_blobContainer.Name != formattedStorageContainerName))
            {
                var cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                _blobContainer = cloudBlobClient.GetContainerReference(formattedStorageContainerName);

                if (!_blobContainer.Exists())
                {
                    try
                    {
                        _blobContainer.Create();

                        while (!_blobContainer.Exists())
                            Thread.Sleep(100);
                    }
                    catch (StorageException storageException)
                    {
                        WriteDebugError(String.Format("NLog.AzureStorage - Failed to create Azure Storage Blob Container '{0}' - Storage Exception: {1} {2}", formattedStorageContainerName, storageException.Message, GetStorageExceptionHttpStatusMessage(storageException)));
                        throw;
                    }
                }
            }
        }

        private void InstanciateStorageBlob(string formattedStorageContainerName, string formattedStorageBlobName)
        {
            if ((_appendBlob == null) || (_appendBlob.Name != formattedStorageBlobName))
            {
                _appendBlob = _blobContainer.GetAppendBlobReference(formattedStorageBlobName);

                if (!_appendBlob.Exists())
                {
                    try
                    {
                        _appendBlob.CreateOrReplace(AccessCondition.GenerateIfNotExistsCondition(), null, null);
                    }
                    catch (StorageException storageException)
                    {
                        WriteDebugError(String.Format("NLog.AzureStorage - Failed to create Azure Storage Append Blob '{0}' in the Container '{1}' - Storage Exception: {2} {3}", formattedStorageBlobName, formattedStorageContainerName, storageException.Message, GetStorageExceptionHttpStatusMessage(storageException)));
                        throw;
                    }
                }
            }
        }

        private static string GetStorageExceptionHttpStatusMessage(StorageException storageException)
        {
            var storageRequestInfo = storageException.RequestInformation;

            if (storageRequestInfo == null)
                return (string.Empty);

            return (storageRequestInfo.HttpStatusMessage);
        }

        private void WriteDebugInfo(string message)
        {
            Trace.WriteLineIf(EnableDebug, String.Concat("INFO: ", message));
        }

        private void WriteDebugError(string message)
        {
            Trace.WriteLineIf(EnableDebug, String.Concat("ERROR: ", message));
        }
    }
}
