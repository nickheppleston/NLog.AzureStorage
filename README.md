NLog.AzureStorage
=================

NLog.AzureStorage is an [Microsoft Azure Storage](https://azure.microsoft.com/en-gb/services/storage) target for [NLog](https://github.com/jkowalski/NLog), allowing you to send log messages to an Azure Storage Endpoint in real-time. Append Blobs are the only supported storage endpoints at this time.

[![NuGet version (NLog.AzureStorage)](https://img.shields.io/nuget/v/NLog.AzureStorage.svg?style=flat)](https://www.nuget.org/packages/NLog.AzureStorage/)
[![NuGet version (NLog.AzureStorage)](https://img.shields.io/nuget/dt/NLog.AzureStorage.svg?style=flat)](https://www.nuget.org/packages/NLog.AzureStorage/)

## Getting Started

See the included samples at [NLog.AzureStorage.Samples.WebAPI](https://github.com/nickheppleston/NLog.AzureStorage/tree/master/src/NLog.AzureStorage.Samples.WebAPI) and [TODO](Cloud Service Worker Role) for examples of running the NLog.AzureStorage Target and having log file output appearing in Azure Blob Storage.

To add to your own projects do the following:

#### Add NLog.AzureStorage.dll to your project(s) via [NuGet](https://www.nuget.org/packages/NLog.AzureStorage/)

	> install-package NLog.AzureStorage

NLog.AzureStorage has dependencies on the following NuGet Packages:

- NLog ≥ 4.1.1
- WindowsAzure.Storage ≥ 5.0.2
	
#### Configure NLog

Add the assembly and new target to NLog.config:

	<?xml version="1.0" encoding="utf-8" ?>
	<nlog 	xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      		xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      		autoReload="true"
      		throwExceptions="false">

      <extensions>
        <add assembly="NLog.AzureStorage" />
      </extensions>

      <targets>
        <target xsi:type="AzureBlobStorageLogger"
            name="BlobStorageLogger"
            storageConnectionString="[AZURE STORAGE CONNECTION STRING]" 
            storageContainerName="nlog-storage-test" 
            storageBlobName="nlog-storage-test.txt" 
          />
      </targets>

      <rules>
        <logger name="*" minlevel="Trace" writeTo="BlobStorageLogger" />
      </rules>
  
Note:  the only required properties for the target are *storageConnectionString*, *storageContainerName* and *storageBlobName*.

## Debugging

By default, no debugging information will be generated when the custom Log Target is used. If you require debug information, this can be enabled by setting the *enableDebug* property to 'true' within the log target configuration as follows:

        <target xsi:type="AzureBlobStorageLogger"
            name="BlobStorageLogger"
            storageConnectionString="[AZURE STORAGE CONNECTION STRING]" 
            storageContainerName="nlog-storage-test" 
            storageBlobName="nlog-storage-test.txt"
            enableDebug="true"
          />

With debugging enable, you will see trace messages similar to the following (use [DebugView](https://technet.microsoft.com/en-us/library/bb896647.aspx) or something similar to view the traces):

	INFO: NLog.AzureStorage - StorageConnectionString: [AZURE STORAGE CONNECTION STRING] 
	INFO: NLog.AzureStorage - StorageContainerName:    nlog-storage-test
	INFO: NLog.AzureStorage - StorageBlobName:         nlog-storage-test.txt
	INFO: NLog.AzureStorage - EnableDebug:             True
	INFO: NLog.AzureStorage - (Internal) FormattedStorageContainerName:  nlog-storage-test
	INFO: NLog.AzureStorage - (Internal) FormattedStorageBlobName:       nlog-storage-test-09-10-2015.txt
	INFO: NLog.AzureStorage - nlog-storage-test/nlog-storage-test.txt - Successfully wrote bytes: 143
	INFO: NLog.AzureStorage - nlog-storage-test/nlog-storage-test.txt - Successfully wrote bytes: 256
	INFO: NLog.AzureStorage - nlog-storage-test/nlog-storage-test.txt - Successfully wrote bytes: 189

For each log message written to Azure Blob Storage, a corresponding '*Successfully wrote bytes: XXX*' line will be output, as shown in the last three lines.

## Feedback

Feel free to tweet [@nickheppleston](http://twitter.com/nickheppleston) for questions or comments on the code.  You can also submit a GitHub issue [here](https://github.com/nickheppleston/NLog.AzureStorage/issues).

## License

https://github.com/nickheppleston/NLog.AzureStorage/blob/master/LICENCE
