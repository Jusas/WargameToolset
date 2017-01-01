using DeckToolbox.WRD.Resolvers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeckToolbox.WRD
{ // Not needed. ISerializable, IDeserializable interfaces with implementation for deck code -> RawDeck + viceversa.
    // Make interfaces for resolving game data values from deck serializer values.
    // Then, we could have deck builder that utilizes deck serializer, resolvers, etc.
    public class Deck : RawDeck
    {
        public string Faction { get; set; }
        public string Country { get; set; }
        public string Coalition { get; set; }
        public string Specialization { get; set; }
        public string Era { get; set; }


        public Deck(
            IDeckValueResolver deckValueResolver, 
            IUnitResolver unitValueResolver
            ) : base()
        {

        }

        public Deck(
            IDeckValueResolver deckValueResolver,
            IUnitResolver unitValueResolver,
            string base64deckString
            ) : base(base64deckString)
        {

        }


    }
}
