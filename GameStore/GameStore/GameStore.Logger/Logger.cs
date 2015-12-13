using System;
using System.Diagnostics;
using GameStore.Models.Utils;
using NLogger = NLog.Logger;

namespace GameStore.Logger
{
    public class Logger : ILogger
    {
        private readonly NLogger _logger = NLog.LogManager.GetCurrentClassLogger();

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogDebug(Exception e)
        {
            _logger.Debug(e, "{0} catched by {1}", e.GetType().Name, GetMethodCallerName());
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogError(Exception e)
        {
            _logger.Error(e, "{0} catched by {1}", e.GetType().Name, GetMethodCallerName());
        }

        public void LogError(Exception e, string message)
        {
            _logger.Error(e, "{0} catched by {1}. Message: {2}.{3}", e.GetType().Name, GetMethodCallerName(), message, Environment.NewLine);
        }

        public void LogFatal(Exception e)
        {
            _logger.Fatal(e);
        }

        public IDisposable LogPerfomance()
        {
            return new PerfomanceLogger(_logger, GetMethodCallerName());
        }
        
        private string GetMethodCallerName()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);
            return sf.GetMethod().ReflectedType.Name + "." + sf.GetMethod().Name;
        }

        private class PerfomanceLogger : IDisposable
        {
            private readonly Stopwatch _stopwatch = new Stopwatch();

            private readonly NLogger _logger;

            private readonly string _methodName;

            public PerfomanceLogger(NLogger logger, string methodName)
            {
                _logger = logger;
                _methodName = methodName;
                _stopwatch.Start();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                _logger.Info(string.Format("Perfomance of {0} is {1} ms", _methodName, _stopwatch.ElapsedMilliseconds));
            }
        }
    }
}
