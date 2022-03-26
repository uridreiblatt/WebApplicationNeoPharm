
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;


namespace WebApplicationNeoPharm.Log
{
     
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddTheCodeFileLogger(this ILoggingBuilder builder, Action<FileLoggerOptions> configure)
        {
            builder.Services.AddSingleton<ILoggerProvider, FileLoggerProvider>();
            builder.Services.Configure(configure);
            return builder;
        }
    }
}
