using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace dotnet_code_challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .AddSingleton<IApplication, BetEasyConsoleApplication>()
                .BuildServiceProvider();


            serviceProvider.GetService<IApplication>().Run(args);
        }
    }
}
