using System;
using NUnit.Framework;

namespace NLog.AzureStorage.Tests
{
    [TestFixture]
    public class AzureBlobStorageProxyTests
    {
        private static bool enableDebug = true;

        #region Container Name Formatting Tests

        // Container names may only contain lowercase letters, numbers, and hyphens and must start with a letter or number.
        // The name cannot contain two consecutive hyphens. The name must also be between 3 and 63 characters long.

        [Test]
        public void ContainerName_Valid_StartsWithLetter()
        {
            // Setup
            var connectionString = String.Empty;
            var containerName = "valid-container-name";
            var storageBlobName = String.Empty;

            // Execution
            var azureBlobStorageProxy = new AzureBlobStorageProxy(connectionString, containerName, storageBlobName, enableDebug);

            // Assertion
            Assert.AreEqual(containerName, azureBlobStorageProxy.StorageContainerName);
        }

        [Test]
        public void ContainerName_Valid_StartsWithNumber()
        {
            // Setup
            var connectionString = String.Empty;
            var containerName = "1-valid-container-name";
            var storageBlobName = String.Empty;

            // Execution
            var azureBlobStorageProxy = new AzureBlobStorageProxy(connectionString, containerName, storageBlobName, enableDebug);

            // Assertion
            Assert.AreEqual(containerName, azureBlobStorageProxy.StorageContainerName);
        }

        [Test]
        public void ContainerName_Valid_ConvertedToLowerCase()
        {
            // Setup
            var connectionString = String.Empty;
            var containerName = "CamelCaseContainerName"; // We expect this container name to be converted to lowercase
            var storageBlobName = String.Empty;

            // Execution
            var azureBlobStorageProxy = new AzureBlobStorageProxy(connectionString, containerName, storageBlobName, enableDebug);

            // Assertion
            Assert.AreEqual(containerName.ToLower(), azureBlobStorageProxy.FormattedStorageContainerName); // Use the (internal) formatted Container Name
        }

        [Test]
        public void ContainerName_Valid_InvalidCharactersRemoved()
        {
            // Setup
            var connectionString = String.Empty;
            var containerName = "ContainerName`¬!\"£$%^&*()_+\\|?/,<.>'@#~[{]}="; // Container names may only contain lowercase letters, numbers, and hyphens
            var storageBlobName = String.Empty;

            // Execution
            var azureBlobStorageProxy = new AzureBlobStorageProxy(connectionString, containerName, storageBlobName, enableDebug);

            // Assertion
            Assert.AreEqual("containername", azureBlobStorageProxy.FormattedStorageContainerName); // Use the (internal) formatted Container Name
        }

        [Test]
        public void ContainerName_Invalid_DoesNotStartWithLetterOrNumber()
        {
            // Setup
            var connectionString = String.Empty;
            var containerName = "CamelCaseContainerName"; // We expect this container name to be converted to lowercase
            var storageBlobName = String.Empty;

            // Execution
            var azureBlobStorageProxy = new AzureBlobStorageProxy(connectionString, containerName, storageBlobName, enableDebug);

            // Assertion
            Assert.AreEqual(containerName.ToLower(), azureBlobStorageProxy.FormattedStorageContainerName); // Use the (internal) formatted Container Name
        }

        #endregion
    }
}
