using System;
using System.Diagnostics;
using NLog.Config;
using NLog.Targets;

namespace NLog.AzureStorage
{
    [Target("AzureBlobStorageLogger")]
    public class AzureBlobStorageLogger : TargetWithLayout
    {
        private AzureBlobStorageProxy _azureBlobStorageProxy;

        [RequiredParameter]
        public string StorageConnectionString { get; set; }

        [RequiredParameter]
        public string StorageContainerName { get; set; }

        [RequiredParameter]
        public string StorageBlobName { get; set; }

        protected override void Write(LogEventInfo logEvent)
        {
            if (!string.IsNullOrEmpty(logEvent.Message))
            {
                InitializeAzureBlobStorageProxy();

                var logMessage = String.Concat(Layout.Render(logEvent), Environment.NewLine);

                _azureBlobStorageProxy.WriteTextToBlob(logMessage);
            }
        }

        private void InitializeAzureBlobStorageProxy()
        {
            if (_azureBlobStorageProxy == null)
            {
                _azureBlobStorageProxy = new AzureBlobStorageProxy(StorageConnectionString, StorageContainerName, StorageBlobName);
            }
            else
            {
                // Handle the LogManager.ReconfigExistingLoggers() scenario - this feels clunky and could probably be made better
                if ((_azureBlobStorageProxy.StorageConnectionString != StorageConnectionString) || (_azureBlobStorageProxy.StorageContainerName != StorageContainerName) || (_azureBlobStorageProxy.StorageBlobName != StorageBlobName))
                {
                    _azureBlobStorageProxy = null;
                    _azureBlobStorageProxy = new AzureBlobStorageProxy(StorageConnectionString, StorageContainerName, StorageBlobName);
                }
            }
        }
    }
}
