using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeckToolbox.WRD.DataModels
{
    public class DecodedUnitCard
    {
        public int Id { get; set; } 
        public int Veterancy { get; set; }
        public virtual int[] Ids => new[] { Id };
    }
}
