using System;

namespace ZEngine.Unity.Core.Logging
{
    public class DefaultCategoryLogger : ILogger
    {
        private readonly string mDefaultCat;
        private readonly ICategoryLogger mCategoryLogger;
        public DefaultCategoryLogger(string defaultCat,
            ICategoryLogger categoryLogger)
        {
            mDefaultCat = defaultCat;
            mCategoryLogger = categoryLogger;
        }

        public void Info(string message)
        {
            mCategoryLogger.Info(mDefaultCat, message);
        }

        public void Warn(string message)
        {
            mCategoryLogger.Warn(mDefaultCat, message);
        }

        public void Error(string message)
        {
            mCategoryLogger.Error(mDefaultCat, message);
        }

        public void Ex(Exception exception)
        {
            mCategoryLogger.Ex(mDefaultCat, exception);
        }
    }
}