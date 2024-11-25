using LogAnalytics.Client;
using System.Runtime.CompilerServices;
using System.Text.Json;
using LogLevel = NLog.LogLevel;

namespace ProductAPINLogAnalytics.Services
{
    #region ILogAnalyticsService
    public interface ILogAnalyticsService
    {
        Task LogObjectAsync(object logEntry);
        Task LogListObjectAsync(List<object> listLogEntry);
    }

    #endregion

    #region LogAnalyticsService
    public class LogAnalyticsService : ILogAnalyticsService
    {
        private readonly LogAnalyticsClient _client;
        private readonly string _logType;

        public LogAnalyticsService(IConfiguration configuration)
        {
            var workspaceId = configuration["LogAnalytics:WorkspaceId"];
            var sharedKey = configuration["LogAnalytics:SharedKey"];
            _logType = configuration["LogAnalytics:LogType"];

            _client = new LogAnalyticsClient(workspaceId, sharedKey);
        }

        public async Task LogObjectAsync(object logEntry)
        {
            try
            {
                await _client.SendLogEntry(logEntry, _logType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending log to Azure: {ex.Message}");
            }
        }

        public async Task LogListObjectAsync(List<object> listLogEntry)
        {
            try
            {
                await _client.SendLogEntries(listLogEntry, _logType);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending log to Azure: {ex.Message}");
            }
        }
    }

    #endregion

    #region LogAnalyticsExtensions
    public static class LogAnalyticsExtensions
    {
        public static object LogInfo(this ILogAnalyticsService logAnalyticsService, string message, object details)
        {
            var logEntry = new
            {
                Level = LogLevel.Info.ToString(),
                Message = message,
                Timestamp = DateTime.UtcNow,
                Details = JsonSerializer.Serialize(details)
            };

            return logEntry;
        }

        public static object LogError(this ILogAnalyticsService logAnalyticsService, string message, Exception ex, object details)
        {
            var logEntry = new
            {
                Level = LogLevel.Error.ToString(),
                Message = message,
                Timestamp = DateTime.UtcNow,
                Details = JsonSerializer.Serialize(details)
            };

            return logEntry;
        }

        public static string FormatMessage(this ILogAnalyticsService logAnalyticsService, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            var className = System.IO.Path.GetFileNameWithoutExtension(filePath);
            var methodName = memberName;
            var line = lineNumber;

            return $"Message: {message} [C]: {className}, [M]: {methodName}, [L]: {line} ";
        }
    }

    #endregion
}
