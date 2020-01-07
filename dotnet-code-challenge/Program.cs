using BetEasy.Caulfield;
using BetEasy.Core.Abstraction;
using BetEasy.Wolverhampton;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

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
                .Configure<FeedSettings>(config.GetSection("FeedSettings"))
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<IApplication, BetEasyConsoleApplication>()
                .AddTransient<CaulfieldRaceFeedReader>()
                .AddTransient<WolverhamptonRaceFeedReader>()
                .AddTransient<ServiceResolver>(provider => key =>
                {
                    switch (key)
                    {
                        case "Caulfield":
                            return provider.GetService<CaulfieldRaceFeedReader>();
                        case "Wolverhampton":
                            return provider.GetService<WolverhamptonRaceFeedReader>();
                        default:
                            return null;
                    }
                })
                .AddLogging(configure => configure.AddConsole())
                .BuildServiceProvider();


            serviceProvider.GetService<IApplication>().Run(args);
        }
    }
}
