using System;
using System.IO;

namespace ZEngine.Unity.Core.Logging
{
    public class FileLogger : ILogger
    {
        private readonly string mFilePath;
        public FileLogger(string filepath)
        {
            mFilePath = filepath;
            
            File.WriteAllText(mFilePath, "");
        }
        public void Info(string message)
        {
            File.AppendAllText(mFilePath, $"{DateTime.Now:s} [INFO] {message}\n");
        }

        public void Warn(string message)
        {
            File.AppendAllText(mFilePath, $"{DateTime.Now:s} [WARN] {message}\n");
        }

        public void Error(string message)
        {
            File.AppendAllText(mFilePath, $"{DateTime.Now:s} [ERROR] {message}\n");
        }

        public void Ex(Exception exception)
        {
            File.AppendAllText(mFilePath, $"{DateTime.Now:s} [EX] {exception}\n");
        }
    }
}