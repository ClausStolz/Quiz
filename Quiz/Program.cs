using System;
using System.IO;

using Quiz.Controllers;
using Microsoft.Extensions.Configuration;


namespace Quiz
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = GetConfiguration("app.config");

            var item = new TcpController(configuration);
            Console.WriteLine("Hello World!");
        }

        private static IConfigurationRoot GetConfiguration(string configName)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(configName);

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

    
    }
}
