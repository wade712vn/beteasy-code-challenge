using System;
using System.Collections.Generic;
using System.Text;

namespace BetEasy.Core.Models
{
    public class Race
    {
        private Horse[] _horses;

        public int Number { get; private set; }
        public string Name { get; private set; }
        public DateTimeOffset StartTime { get; private set; }

        public Race(int number, string name, DateTimeOffset startTime, Horse[] horses)
        {
            Number = number;
            Name = name;
            StartTime = startTime;
            _horses = horses;
        }

        public Horse[] Horses => _horses ?? (_horses = new Horse[0]);
    }
}
