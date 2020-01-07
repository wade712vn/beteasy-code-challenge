using BetEasy.Core.Abstraction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using BetEasy.Caulfield.Models;
using BetEasy.Core.Exceptions;
using Race = BetEasy.Core.Models.Race;

namespace BetEasy.Caulfield
{
    public class CaulfieldRaceFeedReader : IRaceFeedReader
    {
        private static string DateTimeFormat = "dd/MM/yyyy hh:mm:ss tt";

        public Race ReadFromStream(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(Meeting));

            Meeting meeting;
            try
            {
                meeting = (Meeting)serializer.Deserialize(stream);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidFeedException("XML document couldn't be parsed", e);
            }

            var race = meeting.Races.FirstOrDefault();
            if (race == null)
            {
                throw new InvalidFeedException("Feed doesn't contain any race");
            }

            var startTime = DateTimeOffset.ParseExact(
                race.StartTime,
                DateTimeFormat,
                CultureInfo.CurrentCulture);

            var horsePriceMapByNumber = race.Prices
                .SelectMany(y => y.Horses)
                .GroupBy(y => y.Number)
                .ToDictionary(y => y.Key, y => y.First().Price);

            var horses = race.Horses.Select(y => new Core.Models.Horse
            {
                Number = y.Number.ToString(),
                Name = y.Name,
                Price = horsePriceMapByNumber.GetValueOrDefault(y.Number)
            }
            ).ToArray();

            if (!horses.Any())
            {
                throw new InvalidFeedException("Feed doesn't contain any horse");
            }

            return new Race(race.Number, race.Name, startTime, horses);

        }
    }
}
