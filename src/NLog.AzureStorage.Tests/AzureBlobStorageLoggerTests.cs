using System;
using System.Configuration;
using NUnit.Framework;
using NLog.AzureStorage.Tests.Helpers;

namespace NLog.AzureStorage.Tests
{
    [TestFixture]
    public class AzureBlobStorageLoggerTests
    {
        private const string NLOG_TARGET_NAME = "BlobStorageLogger";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly AzureStorageBlobHelpers storageBlobHelpers;
        private readonly string storageContainerName, storageBlobName;

        public AzureBlobStorageLoggerTests()
        {
            var storageConnectionString = ConfigurationManager.ConnectionStrings["AzureStorageConnectionString"].ConnectionString;
            storageBlobHelpers = new AzureStorageBlobHelpers(storageConnectionString);

            storageContainerName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageContainerName;
            storageBlobName = NLogTargetHelpers.GetAzureBlobStorageLoggerTarget(NLOG_TARGET_NAME).StorageBlobName;
        }

        [TestFixtureTearDown]
        public void TestFixtureCleanup()
        {
            storageBlobHelpers.DeleteStorageContainer(storageContainerName);
        }

        [Test]
        public void Log_Trace()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            // Execution
            logger.Trace(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "TRACE", logger.Name, logMessage);
            Assert.IsTrue(storageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Debug()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            // Execution
            logger.Debug(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "DEBUG", logger.Name, logMessage);
            Assert.IsTrue(storageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Info()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            // Execution
            logger.Info(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "INFO", logger.Name, logMessage);
            Assert.IsTrue(storageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Warning()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            // Execution
            logger.Warn(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "WARN", logger.Name, logMessage);
            Assert.IsTrue(storageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Error()
        {
            // Setup
            var logMessage = String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            // Execution
            logger.Error(logMessage);

            // Assertion
            var expectedLogMessage = string.Format("|{0}|{1}|{2}", "ERROR", logger.Name, logMessage);
            Assert.IsTrue(storageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobWithTextExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName), expectedLogMessage), "Missing Log Message");
        }

        [Test]
        public void Log_Multiple_Info()
        {
            // Setup - None

            // Execution
            logger.Info(String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")));
            logger.Info(String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")));
            logger.Info(String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")));
            logger.Info(String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")));
            logger.Info(String.Format("This is a test log message from NLog @ {0}", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff")));

            // Assertion
            Assert.IsTrue(storageBlobHelpers.StorageContainerExists(storageContainerName), "Missing Storage Container");
            Assert.IsTrue(storageBlobHelpers.StorageAppendBlobExists(storageContainerName, AzureStorageBlobHelpers.FormatBlobName(storageBlobName)), "Missing Storage Blob");
        }
    }
}
