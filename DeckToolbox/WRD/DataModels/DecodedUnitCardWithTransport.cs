using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeckToolbox.WRD.DataModels
{
    public class DecodedUnitCardWithSuperTransport : DecodedUnitCardWithTransport
    {
        public int SuperTransportId { get; set; }
        public override int[] Ids => new[] { Id, TransportId, SuperTransportId };
    }
}
