﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebApplicationNeoPharm.Log
{
    public class FileLogger : ILogger
    {
        private readonly object FileLock = new object();
        protected readonly FileLoggerProvider _fileLoggerProvider;

        public FileLogger( FileLoggerProvider fileLoggerProvider)
        {
            _fileLoggerProvider = fileLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
           
            if (!IsEnabled(logLevel))
            {
                return;
            }
            try
            {
                var fullFilePath = _fileLoggerProvider.Options.FolderPath + "/" + _fileLoggerProvider.Options.FilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyyMMdd"));
                var logRecord = string.Format("{0} [{1}] {2} {3}", "[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]", logLevel.ToString(), formatter(state, exception), exception != null ? exception.StackTrace : "");
                lock (FileLock)
                {
                    using (var streamWriter = new StreamWriter(fullFilePath, true))
                    {
                        streamWriter.WriteLine(logRecord);
                    }
                }
            }
            catch (Exception)
            {

                
            }

            
        }
    }
}
