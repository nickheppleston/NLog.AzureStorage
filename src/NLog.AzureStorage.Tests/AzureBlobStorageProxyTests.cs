using System;
using NUnit.Framework;

namespace NLog.AzureStorage.Tests
{
    [TestFixture]
    public class AzureBlobStorageProxyTests
    {
        #region Container Name Formatting Tests

        // Container names may only contain lowercase letters, numbers, and hyphens and must start with a letter or number.
        // The name cannot contain two consecutive hyphens. The name must also be between 3 and 63 characters long.

        [Test]
        public void FormatContainerName_Valid_StartsWithLetter()
        {
            // Setup
            var containerName = "valid-container-name";

            // Execution
            var formattedContainerName = AzureBlobStorageProxy.FormatContainerName(containerName);

            // Assertion
            Assert.AreEqual(containerName, formattedContainerName);
        }

        [Test]
        public void FormatContainerName_Valid_StartsWithNumber()
        {
            // Setup
            var containerName = "1-valid-container-name";

            // Execution
            var formattedContainerName = AzureBlobStorageProxy.FormatContainerName(containerName);

            // Assertion
            Assert.AreEqual(containerName, formattedContainerName);
        }

        [Test]
        public void FormatContainerName_Valid_ConvertedToLowerCase()
        {
            // Setup
            var containerName = "CamelCaseContainerName"; // We expect this container name to be converted to lowercase

            // Execution
            var formattedContainerName = AzureBlobStorageProxy.FormatContainerName(containerName);

            // Assertion
            Assert.AreEqual(containerName.ToLower(), formattedContainerName);
        }

        [Test]
        public void FormatContainerName_Valid_InvalidCharactersRemoved()
        {
            // Setup
            var containerName = "ContainerName`¬!\"£$%^&*()_+\\|?/,<.>'@#~[{]}="; // Container names may only contain lowercase letters, numbers, and hyphens

            // Execution
            var formattedContainerName = AzureBlobStorageProxy.FormatContainerName(containerName);

            // Assertion
            Assert.AreEqual("containername", formattedContainerName);
        }

        #endregion

        #region Blob Name Formatting Tests

        [Test]
        public void FormatBlobName()
        {
            // Setup
            var blobName = "sample-blob-name";

            // Execution
            var formattedBlobName = AzureBlobStorageProxy.FormatBlobName(blobName);

            // Assertion
            Assert.AreEqual(String.Concat(blobName, "-", DateTime.Now.ToString("dd-MM-yyyy")), formattedBlobName);
        }

        [Test]
        public void FormatBlobName_EmptyBlobName()
        {
            // Setup
            var blobName = String.Empty;

            // Execution
            var formattedBlobName = AzureBlobStorageProxy.FormatBlobName(blobName);

            // Assertion
            Assert.AreEqual(String.Concat("-", DateTime.Now.ToString("dd-MM-yyyy")), formattedBlobName);
        }

        #endregion
    }
}