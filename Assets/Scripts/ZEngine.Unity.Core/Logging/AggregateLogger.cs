using System;
using System.Collections.Generic;

namespace ZEngine.Unity.Core.Logging
{
    public class AggregateLogger : ILogger
    {
        private readonly List<ILogger> mLoggers = new List<ILogger>();

        public void Register(ILogger logger)
        {
            mLoggers.Add(logger);
        }
        
        public void Info(string msg)
        {
            foreach (ILogger logger in mLoggers)
            {
                logger.Info(msg);
            }
        }

        public void Warn(string msg)
        {
            foreach (ILogger logger in mLoggers)
            {
                logger.Warn(msg);
            }
        }

        public void Error(string msg)
        {
            foreach (ILogger logger in mLoggers)
            {
                logger.Error(msg);
            }
        }

        public void Ex(Exception ex)
        {
            foreach (ILogger logger in mLoggers)
            {
                logger.Ex(ex);
            }
        }
    }
}