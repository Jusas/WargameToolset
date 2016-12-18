using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeckToolbox.Utils;
using DeckToolbox.WRD;
using Xunit;
using System.Diagnostics;
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
            //string deckString = "@HiMBkykOxZcHZM9REmYhFTMQioSgc2F6BhgYWGBhggXSGQniJJiHdBIKKAxMpmIPQsMruCwSudJ8QVoGcggKbiHU";

            var ds = decks.Split('\n');
            foreach(var d in ds)
            {
                DecodedDeck dk = new DecodedDeck(d);
                //output.WriteLine(dk.NationalityCode.ToString());
                output.WriteLine(string.Join(" ", Convert.ToString(dk.CountryId, 2).PadLeft(10, '0')));
            }

            /*DecodedDeck deck = new DecodedDeck(deckString);
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
            Assert.True(deck.DecodedUnitCards.Count(uc => uc.Id == 23 && uc.Ids.Contains(48) && uc.Ids.Contains(573)) == 1);*/
        }
        
    }
}
