using System;
using System.IO;
using System.Threading.Tasks;

using Quiz.Controllers;
using Quiz.Repositories;
using Quiz.Repositories.Interfaces;

using NLog;
using NLog.Extensions.Logging;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Quiz
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                var configuration = GetConfiguration("app.config");
                var serviceProvider = GetServiceProvider(configuration);

                using (serviceProvider as IDisposable)
                {
                    var tcpController = serviceProvider.GetRequiredService<TcpController>();
                    var median = await tcpController.GetMedian();

                    Console.WriteLine(median.ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }   
        }

        private static IConfiguration GetConfiguration(string configName)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configName, optional: true, reloadOnChange: true);

            if (builder == null)
            {
                throw new Exception("Error in creating builder");
            }

            var configuration = builder.Build();
        
            return configuration switch
            {
                null => throw new Exception("Can not build configuration"),
                _ => configuration
            };
        }

        private static IServiceProvider GetServiceProvider(IConfiguration config)
        {
        return new ServiceCollection()
            .AddSingleton<IConfiguration>(config)
            .AddSingleton<TcpController>()
            .AddTransient<ITcpRepository, TcpRepository>()
            .AddLogging(loggingBuilder =>
            {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    loggingBuilder.AddNLog(config);
            })
            .BuildServiceProvider();
        }

    
    }
}
