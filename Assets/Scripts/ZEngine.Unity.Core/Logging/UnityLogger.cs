using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Logging
{
    public class UnityLogger : ILogger
    {
        public void Info(string message)
        {
            Debug.Log(message);
        }

        public void Warn(string message)
        {
            Debug.LogWarning(message);
        }

        public void Error(string message)
        {
            Debug.LogError(message);
        }

        public void Ex(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}