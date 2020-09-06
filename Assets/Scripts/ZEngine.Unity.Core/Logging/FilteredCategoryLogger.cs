using System;
using System.Collections.Generic;

namespace ZEngine.Unity.Core.Logging
{
    public class FilteredCategoryLogger : ICategoryLogger
    {
        private readonly ILogger mLogger;
        
        private readonly HashSet<string> mFilteredCats;
        private readonly HashSet<string> mFilteredErrorCats;
        private readonly FormatMessage mFormatMessage;

        public delegate string FormatMessage(string cat, string message);
        public FilteredCategoryLogger(ILogger logger, 
            FormatMessage formatMessage,
            HashSet<string> filteredCats,
            HashSet<string> filteredErrorCats)
        {
            mLogger = logger;
            mFormatMessage = formatMessage;
            mFilteredCats = filteredCats;
            mFilteredErrorCats = filteredErrorCats;
        }
        
        public void Info(string cat, string message)
        {
            if (mFilteredCats.Contains(cat))
            {
                return;
            }

            string msg = mFormatMessage(cat, message);
        
            mLogger.Info(msg);
        }

        public void Warn(string cat, string message)
        {
            if (mFilteredCats.Contains(cat))
            {
                return;
            }

            string msg = mFormatMessage(cat, message);
        
            mLogger.Warn(msg);
        }

        public void Error(string cat, string message)
        {
            if (mFilteredErrorCats.Contains(cat))
            {
                return;
            }

            string msg = mFormatMessage(cat, message);
        
            mLogger.Error(msg);
        }

        public void Ex(string cat, Exception exception)
        {
            mLogger.Ex(exception);
        }
    }
}