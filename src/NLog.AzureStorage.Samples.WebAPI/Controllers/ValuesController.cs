using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NLog.AzureStorage.Samples.WebAPI.Controllers
{
    public class LoggingController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Random _generator = new Random();

        // GET api/logging
        public string Get()
        {
            var index = _generator.Next(0, Messages.Length);
            var message = Messages[index];

            logger.Log(message.Level, message.Message);

            return (String.Format("The following log message has been written to the configured NLog.AzureStorage Target: '{0}'", message));
        }

        public LogEventInfo[] Messages =
        {
            new LogEventInfo{Level = LogLevel.Info, Message = "NLog.AzureStorage Contrived Logging Example #1 - Hello, world!"},
            new LogEventInfo{Level = LogLevel.Error, Message = "NLog.AzureStorage Contrived Logging Example #2 - There is an example server issue."},
            new LogEventInfo{Level = LogLevel.Trace, Message = "NLog.AzureStorage Contrived Logging Example #3 - You have entered the api/logging API method."}
        };
    }
}
