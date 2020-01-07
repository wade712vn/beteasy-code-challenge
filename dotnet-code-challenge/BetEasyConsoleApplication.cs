using BetEasy.Core.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BetEasy.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dotnet_code_challenge
{
    public class BetEasyConsoleApplication : IApplication
    {
        private static string FeedNameRegex = @"([A-Za-z]+)_([A-Za-z0-9]+)\.(json|xml)$";

        private readonly FeedSettings _feedSettings;
        private readonly ServiceResolver _serviceAccessor;

        private readonly ILogger<BetEasyConsoleApplication> _logger;

        public BetEasyConsoleApplication(IOptions<FeedSettings> settings, ServiceResolver serviceAccessor, ILogger<BetEasyConsoleApplication> logger)
        {
            _feedSettings = settings.Value;
            _serviceAccessor = serviceAccessor;
            _logger = logger;
        }

        public void Run(string[] args)
        {
            var feedDataPath = _feedSettings.Folder;

            var feedDataFolder = new DirectoryInfo(feedDataPath);

            foreach (var file in feedDataFolder.GetFiles())
            {
                try
                {
                    ProcessFeed(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error occurred reading feed {file.Name} - {e.Message}");
                    _logger.LogError(e, $"Error occurred reading feed {file.Name} - {e.Message}");
                }

                // Print empty line
                Console.WriteLine();
            }
        }

        private void ProcessFeed(FileInfo file)
        {
            var match = Regex.Match(file.Name, FeedNameRegex);
            if (!match.Success)
            {
                throw new Exception("Feed name has invalid format");
            }

            var providerName = match.Groups[1].Value;
            var reader = _serviceAccessor(providerName);
            if (reader == null)
            {
                throw new Exception($"No feed reader found for provider {providerName}");
            }

            Console.WriteLine($"Reading data feed {file.Name}");

            Race race;
            using (var stream = new FileStream(file.FullName, FileMode.Open))
            {
                race = reader.ReadFromStream(stream);
            }

            Console.WriteLine($"Printing horse names for race {race.Name}");

            Console.WriteLine("Name \t Price");
            foreach (var horse in race.Horses.OrderBy(x => x.Price))
            {
                Console.WriteLine($"{horse.Name} \t {horse.Price}");
            }
        }
    }
}
