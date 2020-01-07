using BetEasy.Core.Abstraction;
using BetEasy.Wolverhampton.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BetEasy.Core.Exceptions;

namespace BetEasy.Wolverhampton
{
    public class WolverhamptonRaceFeedReader : IRaceFeedReader
    {
        public Core.Models.Race ReadFromStream(Stream stream)
        {
            var serializer = new JsonSerializer();

            Race race;
            try
            {
                using (var sr = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    race = serializer.Deserialize<Race>(jsonTextReader);
                }
            }
            catch (JsonReaderException e)
            {
                throw new InvalidFeedException("JSON document couldn't be parsed", e);
            }
            catch (JsonSerializationException e)
            {
                // JSON document is valid but data has wrong schema
                throw new InvalidFeedException("Data feed has invalid format", e);
            }

            var raceData = race.RawData;

            var market = raceData.Markets.FirstOrDefault();

            var horsePriceMapByNumber = market.Selections
                .GroupBy(y => y.ParticipantNumber)
                .ToDictionary(y => y.Key, y => y.First().Price);

            var horses = raceData.Participants.Select(x => new Core.Models.Horse
            {
                Name = x.Name,
                Number = x.Number,
                Price = horsePriceMapByNumber.GetValueOrDefault(x.Number)
            }).ToArray();

            if (!horses.Any())
            {
                throw new InvalidFeedException("Feed doesn't contain any horse");
            }

            return new Core.Models.Race(raceData.Sequence, raceData.FixtureName, raceData.StartTime, horses);
        }
    }
}
