using System;
using System.Collections.Generic;
using System.Text;

namespace BetEasy.Wolverhampton.Models
{
    public class Race
    {
        public RaceData RawData { get; set; }
    }

    public class RaceData
    {
        private List<Participant> _participants;
        private Dictionary<string, string> _tags;
        private List<Market> _markets;

        public string FixtureName { get; set; }
        public int Sequence { get; internal set; }
        public DateTimeOffset StartTime { get; set; }

        public List<Market> Markets
        {
            get => _markets ?? (_markets = new List<Market>());
            set => _markets = value;
        }

        public List<Participant> Participants
        {
            get => _participants ?? (_participants = new List<Participant>());
            set => _participants = value;
        }

        public Dictionary<string, string> Tags
        {
            get => _tags ?? (_tags = new Dictionary<string, string>());
            set => _tags = value;
        }
    }

    public class Market
    {
        private List<Selection> _selections;
        private Dictionary<string, string> _tags;

        public List<Selection> Selections
        {
            get => _selections ?? (_selections = new List<Selection>());
            set => _selections = value;
        }

        public Dictionary<string, string> Tags
        {
            get => _tags ?? (_tags = new Dictionary<string, string>());
            set => _tags = value;
        }
    }

    public class Selection
    {
        private Dictionary<string, string> _tags;

        public string Id { get; set; }
        public decimal Price { get; set; }

        public Dictionary<string, string> Tags
        {
            get => _tags ?? (_tags = new Dictionary<string, string>());
            set => _tags = value;
        }

        public string ParticipantNumber => Tags.GetValueOrDefault("participant");
    }

    public class Participant
    {
        private Dictionary<string, string> _tags;

        public int Id { get; set; }
        public string Name { get; set; }

        public Dictionary<string, string> Tags
        {
            get => _tags ?? (_tags = new Dictionary<string, string>());
            set => _tags = value;
        }

        public string Number => Tags.GetValueOrDefault("Number");
    }
}
