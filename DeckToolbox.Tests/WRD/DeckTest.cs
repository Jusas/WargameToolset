using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeckToolbox.Utils;
using DeckToolbox.WRD;
using Xunit;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using DeckToolbox.WRD.Resolvers;
using Newtonsoft.Json.Serialization;
using Xunit.Abstractions;

namespace DeckToolbox.Tests
{
    
    public class DeckTest
    {
        private readonly ITestOutputHelper output;

        public DeckTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        // Sample deck, taken 30.11.2016.
        // CMW ARMORED:
        // CMD CHIEFTAIN Mk.10, HARDENED
        // FUSILIERS '90 (FV432), HARDENED
        // MILAN 2 (FV432), TRAINED
        // CHIEFTAIN MARKSMAN, TRAINED
        // CHALLENGER 2, VETERAN
        // GAZELLE AH.1, TRAINED
        // CENTURION AVRE, HARDENED
        // LYNX AH.7 TOW 2, HARDENED
        // F/A-18A HORNET, TRAINED
        // FREMANTLE, ROOKIE
        // COMMANDOS (STOLLY) (TRANSPORT), HARDENED
        [Fact]
        public void ParseSampleDeck()
        {
            //string deckString = "@GiMIkBcGCPUykOxZcHZheIZEEgrnSfEFYpuCOQ==";
            string decks = @"@AM8AEFA=
@As8AEEg=
@BM8ASAFCgA==
@Bs8ASa0+IA==
@CM8ASDlMAA==
@Cs8AEEc=
@DM8ASx1OYA==
@Ds8ASxxUIA==
@EM8AEPs=
@Es8ASKROQA==
@FM8ASbRUgA==
@Fs8AC90=
@GM8ATA+HwA==
@Hg8ASa1NoA==
@Hh8ASxxUIA==
@Hi8ASDkG4A==
@Hj8ASbRUgA==
@Hm8ASa1NoA==
@Ho8AEUA=
@Hp8ASa1NoA==
@QM8ASm0fYA==
@Qs8AEHw=
@RM8ASmwdwA==
@TM8ACyA=
@Ts8AS55xgA==
@Rs8AErQ=
@SM8ACGk=
@Ss8ASLZAIA==
@Ul8ASm0fYA==
@Uk8ASLZAIA==
@Uq8AS2VlQA==
@Ur8AErQ=";
            string deckString = "@HiMBkykOxZcHZM9REmYhFTMQioSgc2F6BhgYWGBhggXSGQniJJiHdBIKKAxMpmIPQsMruCwSudJ8QVoGcggKbiHU";

            RawDeck deck = new RawDeck(deckString);
            Assert.True(deck.DecodedUnitCards.Count == 11);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 72) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 809 && uc.Ids.Contains(118)) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 814 && uc.Ids.Contains(118)) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 100) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 94) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 622) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 86) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 636) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 743) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 569) == 1);
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 23 && uc.Ids.Contains(48) && uc.Ids.Contains(573)) == 1);
        }

        [Fact]
        public void ResolveRawDeckInfo()
        {
            string deckString = "@HiMBkykOxZcHZM9REmYhFTMQioSgc2F6BhgYWGBhggXSGQniJJiHdBIKKAxMpmIPQsMruCwSudJ8QVoGcggKbiHU";
            var location = Path.Combine(Path.GetDirectoryName(typeof(RawDeck).GetTypeInfo().Assembly.Location), "WRD", "TestData");

            // Using embedded test files because of a bug in visual studio 2017 rc.
            var files = GetType().GetTypeInfo().Assembly.GetManifestResourceNames();
            var jsonFileReader = new AssemblyJsonFileReader(GetType().GetTypeInfo().Assembly);

            JsonFileSourceDeckValueResolver dvr = new JsonFileSourceDeckValueResolver(jsonFileReader);
            var ur = new JsonFileSourceUnitResolver(jsonFileReader);

            dvr.JsonSourceFiles = files;
            dvr.Initialize();

            ur.JsonSourceFiles = dvr.JsonSourceFiles.Where(x => x.Contains("Unit")).ToList();
            ur.Initialize();

            RawDeck deck = new RawDeck(deckString);
            var coalition = dvr.GetCoalitionName(deck.CoalitionId);
            var country = dvr.GetCountryName(deck.FactionId, deck.CountryId);
            var cat = dvr.GetDeckEraName(deck.CategoryId);
            var spec = dvr.GetDeckSpecializationName(deck.UnitTypeId);
            var fac = dvr.GetFactionName(deck.FactionId);

            var ud = ur.GetUnitData(deck.FactionId, deck.DecodedUnitCards.First().Id);

            var ud2 = ur.GetUnitData(17870);
        }

    }
}
