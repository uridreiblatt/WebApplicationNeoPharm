using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebApplicationNeoPharm.Log;

namespace WebApplicationNeoPharm.Log
{
    
    [ProviderAlias("FileLoggerExt")]
    public class FileLoggerProvider : ILoggerProvider
    {
        public readonly FileLoggerOptions Options;

        public FileLoggerProvider(IOptions<FileLoggerOptions> _options)
        {
            Options = _options.Value;

            if (!Directory.Exists(Options.FolderPath))
            {
                Directory.CreateDirectory(Options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
