using Moq;
using RockLib.Logging;

namespace RITA.WebAPI.UnitTests.Utilities
{
    internal static class LoggerUtility
    {
        public static void VerifyException(string message, Mock<ILogger> logger)
        {
            logger
                .Verify(x =>
                    x.Log(It.Is<LogEntry>(le =>
                        le.Level == LogLevel.Error && le.Message.Contains(message)),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<int>()),
                    Times.Once());
        }
    }
}
