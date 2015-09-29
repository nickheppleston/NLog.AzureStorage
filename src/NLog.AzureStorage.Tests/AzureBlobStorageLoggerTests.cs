using System;
using NLog.AzureStorage.Tests.Helpers;
using NUnit.Framework;

namespace NLog.AzureStorage.Tests
{
    [TestFixture]
    public class AzureBlobStorageLoggerTests
    {
        private const string NLOG_TARGET_NAME = "BlobStorageLogger";

        private AzureStorageBlobHelpers _azureStorageBlobHelpers;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AzureBlobStorageLoggerTests()
        {
            _azureStorageBlobHelpers = new AzureStorageBlobHelpers();
        }

        [TestFixtureTearDown]
        public void TestFixtureCleanup()
        {
            var storageContainerName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageContainerName;

            // Comment-out this line to see the Storage Container within the Azure Portal
            _azureStorageBlobHelpers.DeleteStorageContainer(storageContainerName);
        }

        [Test]
        public void Log_Trace()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            var storageContainerName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageContainerName;
            var storageBlobName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageBlobName;

            // Execution
            logger.Trace(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "TRACE", logger.Name, logMessage);
            Assert.IsTrue(_azureStorageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Debug()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            var storageContainerName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageContainerName;
            var storageBlobName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageBlobName;

            // Execution
            logger.Debug(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "DEBUG", logger.Name, logMessage);
            Assert.IsTrue(_azureStorageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Info()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            var storageContainerName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageContainerName;
            var storageBlobName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageBlobName;

            // Execution
            logger.Info(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "INFO", logger.Name, logMessage);
            Assert.IsTrue(_azureStorageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Warning()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            var storageContainerName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageContainerName;
            var storageBlobName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageBlobName;

            // Execution
            logger.Warn(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "WARN", logger.Name, logMessage);
            Assert.IsTrue(_azureStorageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Error()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
            var storageContainerName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageContainerName;
            var storageBlobName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageBlobName;

            // Execution
            logger.Error(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "ERROR", logger.Name, logMessage);
            Assert.IsTrue(_azureStorageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(_azureStorageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }
    }
}
