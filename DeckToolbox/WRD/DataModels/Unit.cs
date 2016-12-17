using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DeckToolbox.WRD.DataModels
{
    public class Unit
    {
        public uint Id { get; set; }        
        public string Name { get; set; }
        public uint DeckId { get; set; }
        public string UnitCategory { get; set; }
        public string Nationality { get; set; }
        public string Faction { get; set; }

        public int[] MaxDeployableAmount { get; set; }
        public int ProductionPrice { get; set; }
        public int MaxPacks { get; set; }
        public int IconType { get; set; }
    }
}
