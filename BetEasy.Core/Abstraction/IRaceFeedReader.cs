using System.IO;
using BetEasy.Core.Models;

namespace BetEasy.Core.Abstraction
{
    public interface IRaceFeedReader
    {
        Race ReadFromStream(Stream stream);
    }

    public delegate IRaceFeedReader ServiceResolver(string key);
}