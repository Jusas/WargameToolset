using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeckToolbox.Utils;
using DeckToolbox.WRD.DataModels;

namespace DeckToolbox.WRD
{
    // http://forums.eugensystems.com/viewtopic.php?p=798441#p798441

    public class DecodedDeck
    {
        public string SourceDeckString { get; private set; }

        public int FactionId { get; set; }
        public int CountryId { get; set; }
        public int CoalitionId { get; set; }
        public int UnitTypeId { get; set; } // Aka deck specialization
        public int CategoryId { get; set; } // Aka era (cat a, cat b, cat c)

        public List<DecodedUnitCard> DecodedUnitCards { get; set; }

        public DecodedDeck(string base64DeckString)
        {
            BuildFromDeckString(base64DeckString);
        }
    
        public DecodedDeck()
        {
            DecodedUnitCards = new List<DecodedUnitCard>();
        }
        

        public void BuildFromDeckString(string base64DeckString)
        {
            // New deck codes start with @, probably to tell the game that they're in the new format.

            base64DeckString = base64DeckString.TrimStart('@');
            SourceDeckString = base64DeckString;
            DecodedUnitCards = new List<DecodedUnitCard>();
            int bitOffset = 0;
            var dataBytes = Convert.FromBase64String(base64DeckString);

            FactionId = (int)dataBytes.GetBitSection(0, 2);
            bitOffset += 2;
            
            // Country and coalition, 10 bits.
            // Note: Used to be 9 bits, did they run out of space?
            CountryId = (int)dataBytes.GetBitSection(bitOffset, 5);
            bitOffset += 5;

            CoalitionId = (int)dataBytes.GetBitSection(bitOffset, 5);
            bitOffset += 5;

            UnitTypeId = (int)dataBytes.GetBitSection(bitOffset, 3);
            bitOffset += 3;

            CategoryId = (int)dataBytes.GetBitSection(bitOffset, 2);
            bitOffset += 2;

            int numUnitsWithTwoTransports = (int)dataBytes.GetBitSection(bitOffset, 4);
            bitOffset += 4;
            int numUnitsWithOneTransport = (int)dataBytes.GetBitSection(bitOffset, 5);
            bitOffset += 5;
                
            for (var i = 0; i < numUnitsWithTwoTransports; i++)
            {
                // The "SuperTransport" (that's what they're called in TShowRoomDeckSerializer) sets are made up of 3 bits for veterancy, 11 bits for the unit, 11 bits for their transport and another 11 points for the landing craft.
                // Note: Used to be 10 bits for unit ids, now is 11 bits. I guess they ran out of room with all the new DLC units.
                var veterancy = (int)dataBytes.GetBitSection(bitOffset, 3);
                bitOffset += 3;
                var unitId = (int)dataBytes.GetBitSection(bitOffset, 11);
                bitOffset += 11;
                var transportId = (int)dataBytes.GetBitSection(bitOffset, 11);
                bitOffset += 11;
                var superTransportId = (int)dataBytes.GetBitSection(bitOffset, 11);
                bitOffset += 11;

                var unitCard = new DecodedUnitCardWithSuperTransport()
                {
                    Id = unitId,
                    SuperTransportId = superTransportId,
                    TransportId = transportId,
                    Veterancy = veterancy
                };
                DecodedUnitCards.Add(unitCard);
            }
                
            for (var i = 0; i < numUnitsWithOneTransport; i++)
            {
                // Transports are 23-bit numbers that are a 3-bit veterancy, 10-bit unit ID, and 10-bit transport ID.
                var veterancy = (int)dataBytes.GetBitSection(bitOffset, 3);
                bitOffset += 3;
                var unitId = (int)dataBytes.GetBitSection(bitOffset, 11);
                bitOffset += 11;
                var transportId = (int)dataBytes.GetBitSection(bitOffset, 11);
                bitOffset += 11;

                var unitCard = new DecodedUnitCardWithTransport()
                {
                    Id = unitId,
                    TransportId = transportId,
                    Veterancy = veterancy
                };
                DecodedUnitCards.Add(unitCard);
            }

            var totalBits = dataBytes.Length*8;
            var remaining = totalBits - bitOffset;
            var numUnitsWithoutTransports = remaining/14; // 13 bits per card
                
            for (var i = 0; i < numUnitsWithoutTransports; i++)
            {
                // Units are composed of a 13-bit identifier, with the first 3 bits being veterancy (0-4, values above 4 cause it to reject the deck) and the next 10 being a unit ID
                var veterancy = (int)dataBytes.GetBitSection(bitOffset, 3);
                bitOffset += 3;
                var unitId = (int)dataBytes.GetBitSection(bitOffset, 11);
                bitOffset += 11;

                var unitCard = new DecodedUnitCard()
                {
                    Id = unitId,
                    Veterancy = veterancy
                };
                DecodedUnitCards.Add(unitCard);
            }
            
        }

        public IList<int> GetAllUnitCardUnitIds()
        {
            List<int> ids = new List<int>();
            DecodedUnitCards.ForEach(uc => ids.AddRange(uc.Ids));            
            return ids.Distinct().ToList();
        }
    }
}
