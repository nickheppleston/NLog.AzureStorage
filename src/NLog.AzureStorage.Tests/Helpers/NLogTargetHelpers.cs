
namespace NLog.AzureStorage.Tests.Helpers
{
    public static class NLogTargetHelpers
    {
        public static AzureBlobStorageLogger GetAzureBlobStorageLoggerTarget(string targetName)
        {
            return ((AzureBlobStorageLogger)LogManager.Configuration.FindTargetByName(targetName));
        }
    }
}