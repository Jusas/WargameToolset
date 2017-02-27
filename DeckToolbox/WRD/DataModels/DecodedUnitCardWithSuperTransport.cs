using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeckToolbox.WRD.DataModels
{
    public class DecodedUnitCardWithTransport : DecodedUnitCard
    {
        public int TransportId { get; set; }
        public override int[] Ids => new[] { Id, TransportId };
    }
}
