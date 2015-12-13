using System;

namespace GameStore.Models.Utils
{
    public interface ILogger
    {
        void LogDebug(string message);

        void LogDebug(Exception e);

        void LogInfo(string message);
        
        void LogError(Exception e);

        void LogError(Exception e, string message);
        
        void LogFatal(Exception e);

        IDisposable LogPerfomance();
    }
}