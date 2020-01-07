using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BetEasy.Caulfield.Models
{
    [XmlRoot("meeting")]
    public class Meeting
    {
        [XmlElement("date")]
        public string Date { get; set; }

        [XmlArray("races")]
        [XmlArrayItem("race", typeof(Race))]
        public List<Race> Races { get; set; }
    }

    public class Race
    {
        private List<Price> _prices;
        private List<Horse> _horses;

        [XmlAttribute("number")]
        public int Number { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("Status")]
        public string Status { get; set; }

        [XmlElement("start_time")]
        public string StartTime { get; set; }

        [XmlArray("horses")]
        [XmlArrayItem("horse", typeof(Horse))]
        public List<Horse> Horses
        {
            get => _horses ?? (_horses = new List<Horse>());
            set => _horses = value;
        }

        [XmlArray("prices")]
        [XmlArrayItem("price", typeof(Price))]
        public List<Price> Prices {
            get => _prices ?? (_prices = new List<Price>());
            set => _prices = value;
        }

    }

    public class Horse
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        public string Country { get; set; }
        public string Sex { get; set; }

        [XmlElement("number")]
        public int Number { get; set; }
    }

    public class Price
    {
        private List<HorsePrice> _horses;

        public string PriceType { get; set; }

        [XmlArray("horses")]
        [XmlArrayItem("horse", typeof(HorsePrice))]
        public List<HorsePrice> Horses
        {
            get => _horses ?? (_horses = new List<HorsePrice>());
            set => _horses = value;
        }
    }

    public class HorsePrice
    {
        [XmlAttribute("number")]
        public int Number { get; set; }

        [XmlAttribute("Price")]
        public decimal Price { get; set; }
    }
}
