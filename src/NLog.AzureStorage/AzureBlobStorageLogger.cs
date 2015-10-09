using System;
using System.ComponentModel;
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

        [DefaultValue(false)]
        public bool EnableDebug { get; set; }

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
                _azureBlobStorageProxy = new AzureBlobStorageProxy(StorageConnectionString, StorageContainerName, StorageBlobName, EnableDebug);
            }
            else
            {
                // Handle the LogManager.ReconfigExistingLoggers() scenario - this feels clunky and could probably be made better - 
                // unfortunately, there doesn't appear to be an effective way to detect whether this method has been called.
                if (LogTargetConfigurationChanged())
                {
                    _azureBlobStorageProxy = null;
                    _azureBlobStorageProxy = new AzureBlobStorageProxy(StorageConnectionString, StorageContainerName, StorageBlobName, EnableDebug);
                }
            }
        }

        private bool LogTargetConfigurationChanged()
        {
            if ((_azureBlobStorageProxy.StorageConnectionString != StorageConnectionString) || 
                (_azureBlobStorageProxy.StorageContainerName != StorageContainerName) || 
                (_azureBlobStorageProxy.StorageBlobName != StorageBlobName) ||
                (_azureBlobStorageProxy.EnableDebug != EnableDebug))
            {
                return (true);
            }

            return (false);
        }
    }
}
