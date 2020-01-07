using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BetEasy.Caulfield;
using BetEasy.Core.Exceptions;
using Xunit;

namespace dotnet_code_challenge.Test.Caulfield
{
    public class CaulfieldRaceFeedReaderTests : IClassFixture<CaulfieldFeedFixture>
    {
        CaulfieldFeedFixture _fixture;

        public CaulfieldRaceFeedReaderTests(CaulfieldFeedFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ReadFromStream_FeedIsValid_ReturnsRace()
        {

            var feedReader = new CaulfieldRaceFeedReader();
            var race = feedReader.ReadFromStream(_fixture.FeedValid);

            Assert.NotNull(race);
            Assert.NotEmpty(race.Horses);
        }

        [Fact]
        public void ReadFromStream_FeedThreeHorses_ReturnsRaceWithThreeHorses()
        {
            var feedReader = new CaulfieldRaceFeedReader();
            var race = feedReader.ReadFromStream(_fixture.FeedThreeHorses);

            Assert.NotNull(race);
            Assert.NotEmpty(race.Horses);
            Assert.Equal(3, race.Horses.Length);
        }

        [Fact]
        public void ReadFromStream_FeedInvalidXml_ThrowsException()
        {

            var feedReader = new CaulfieldRaceFeedReader();

            Exception ex = Assert.Throws<InvalidFeedException>(() => feedReader.ReadFromStream(_fixture.FeedInvalidXml));
            Assert.IsType<InvalidOperationException>(ex.InnerException);
        }

        [Fact]
        public void ReadFromStream_FeedHasNoRace_ThrowsException()
        {

            var feedReader = new CaulfieldRaceFeedReader();

            Exception ex = Assert.Throws<InvalidFeedException>(() => feedReader.ReadFromStream(_fixture.FeedNoRace));
        }
    }

    public class CaulfieldFeedFixture : IDisposable
    {
        public Stream FeedValid { get; }
        public Stream FeedTwoHorses { get; }
        public Stream FeedThreeHorses { get; }
        public Stream FeedInvalidXml { get; }
        public Stream FeedNoRace { get; }

        public CaulfieldFeedFixture()
        {
            FeedValid = new FileStream("TestFeed/Caulfield_Race_Valid.xml", FileMode.Open, FileAccess.Read);
            FeedTwoHorses = new FileStream("TestFeed/Caulfield_Race_TwoHorses.xml", FileMode.Open, FileAccess.Read);
            FeedThreeHorses = new FileStream("TestFeed/Caulfield_Race_ThreeHorses.xml", FileMode.Open, FileAccess.Read);
            FeedInvalidXml = new FileStream("TestFeed/Caulfield_Race_InvalidXml.xml", FileMode.Open, FileAccess.Read);
            FeedNoRace = new FileStream("TestFeed/Caulfield_Race_NoRace.xml", FileMode.Open, FileAccess.Read);
        }

        public void Dispose()
        {
            FeedValid?.Dispose();
            FeedTwoHorses?.Dispose();
            FeedThreeHorses?.Dispose();
            FeedInvalidXml?.Dispose();
            FeedNoRace?.Dispose();
        }
    }
}
