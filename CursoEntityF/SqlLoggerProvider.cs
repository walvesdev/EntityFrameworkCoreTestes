using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CursoEntityF
{
    public static class SqlLoggerProvider 
    {

        internal class SqlServerLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                Console.WriteLine("");
                Console.WriteLine(formatter(state, exception));
                Console.WriteLine("");
            }
        }

        internal class NullLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                //não faz nada
            }
        }

        internal class SqlServerLoggerProvider : ILoggerProvider
        {
            private IList<string> categoriasASeremLogadas = new List<string>
            {
                DbLoggerCategory.Model.Name,
                DbLoggerCategory.Database.Command.Name,
                DbLoggerCategory.Model.Validation.Name
            };

            public static SqlServerLoggerProvider Create()
            {
                return new SqlServerLoggerProvider();
            }

            public ILogger CreateLogger(string categoryName)
            {
                if (categoriasASeremLogadas.Contains(categoryName))
                {
                    return new SqlServerLogger() 
                        ;
                }
                return new NullLogger();
            }

            public void Dispose()
            {

            }
        }

        public static void LogSQLToConsole(this DbContext contexto)
        {
            var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(SqlServerLoggerProvider.Create());
        }

        
    }
}