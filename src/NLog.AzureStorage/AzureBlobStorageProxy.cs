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

        internal string FormattedStorageContainerName { get; private set; }

        internal string FormattedStorageBlobName { get; private set; }

        public AzureBlobStorageProxy(string storageConnectionString, string storageContainerName, string storageBlobName, bool enableDebug)
        {
            StorageConnectionString = storageConnectionString;
            StorageContainerName = storageContainerName;
            StorageBlobName = storageBlobName;
            EnableDebug = enableDebug;
            FormattedStorageContainerName = FormatContainerName(storageContainerName);
            FormattedStorageBlobName = FormatBlobName(storageBlobName);

            WriteDebugInfo(String.Format("NLog.AzureStorage - StorageConnectionString: {0}", StorageConnectionString));
            WriteDebugInfo(String.Format("NLog.AzureStorage - StorageContainerName:    {0}", StorageContainerName));
            WriteDebugInfo(String.Format("NLog.AzureStorage - StorageBlobName:         {0}", StorageBlobName));
            WriteDebugInfo(String.Format("NLog.AzureStorage - EnableDebug:             {0}", EnableDebug));
            WriteDebugInfo(String.Format("NLog.AzureStorage - (Internal) FormattedStorageContainerName:  {0}", FormattedStorageContainerName));
            WriteDebugInfo(String.Format("NLog.AzureStorage - (Internal) FormattedStorageBlobName:       {0}", FormattedStorageBlobName));
        }

        public void WriteTextToBlob(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                InstanciateStorageContainer();
                InstanciateStorageBlob();

                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(text)))
                {
                    WriteDebugInfo(String.Format("NLog.AzureStorage - {0}/{1} - Successfully wrote bytes: {2}", StorageContainerName, StorageBlobName, memoryStream.Length));

                    _appendBlob.AppendBlock(memoryStream);
                }
            }
        }

        private void InstanciateStorageContainer()
        {
            if (_blobContainer == null)
            {
                var cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                _blobContainer = cloudBlobClient.GetContainerReference(FormattedStorageContainerName);

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
                        WriteDebugError(String.Format("NLog.AzureStorage - Failed to create Azure Storage Blob Container '{0}' - Storage Exception: {1} {2}", FormattedStorageContainerName, storageException.Message, GetStorageExceptionHttpStatusMessage(storageException)));
                        throw;
                    }
                }
            }
        }

        private void InstanciateStorageBlob()
        {
            if (_appendBlob == null)
            {
                _appendBlob = _blobContainer.GetAppendBlobReference(FormattedStorageBlobName);

                if (!_appendBlob.Exists())
                {
                    try
                    {
                        _appendBlob.CreateOrReplace(AccessCondition.GenerateIfNotExistsCondition(), null, null);
                    }
                    catch (StorageException storageException)
                    {
                        WriteDebugError(String.Format("NLog.AzureStorage - Failed to create Azure Storage Append Blob '{0}' in the Container '{1}' - Storage Exception: {2} {3}", FormattedStorageBlobName, FormattedStorageContainerName, storageException.Message, GetStorageExceptionHttpStatusMessage(storageException)));
                        throw;
                    }
                }
            }
        }

        private static string FormatBlobName(string blobName)
        {
            return (String.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(blobName), DateTime.Now.ToString("dd-MM-yyyy"), Path.GetExtension(blobName)));
        }

        private static string FormatContainerName(string containerName)
        {
            // Container names may only contain lowercase letters, numbers, and hyphens and must start with a letter or number.
            // The name cannot contain two consecutive hyphens. The name must also be between 3 and 63 characters long.

            var containerNameInvalidCharactersRemoved = Regex.Replace(containerName, "[^abcdefghijklmnopqrstuvwxyz0123456789-]", String.Empty, RegexOptions.IgnoreCase);

            return (containerNameInvalidCharactersRemoved.ToLower());
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
