using DeckToolbox.WRD.Resolvers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeckToolbox.WRD
{
    public class Deck : DecodedDeck
    {
        public string Faction { get; set; }
        public string Nationality { get; set; }
        public string Specialization { get; set; }
        public string Era { get; set; }


        public Deck(
            IDeckValueResolver deckValueResolver, 
            IUnitValueResolver unitValueResolver
            ) : base()
        {

        }

        public Deck(
            IDeckValueResolver deckValueResolver,
            IUnitValueResolver unitValueResolver,
            string base64deckString
            ) : base(base64deckString)
        {

        }


    }
}
