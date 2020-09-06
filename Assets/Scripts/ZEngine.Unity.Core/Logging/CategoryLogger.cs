using System;

namespace ZEngine.Unity.Core.Logging
{
    public interface ICategoryLogger
    {
        void Info(string cat, string message);
        void Warn(string cat, string message);
        void Error(string cat, string message);
        void Ex(string cat, Exception exception);
    }
}