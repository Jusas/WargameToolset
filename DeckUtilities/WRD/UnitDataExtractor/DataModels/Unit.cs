using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitExtractor.DataModels
{
    internal class Unit
    {
        public uint Id { get; set; }
        public string AliasName { get; set; }
        public string Name { get; set; }
        public uint DeckId { get; set; }
        public int Factory { get; set; }
        public string MotherCountry { get; set; }
        public int[] MaxDeployableAmount { get; set; }
        public int ProductionPrice { get; set; }
        public int MaxPacks { get; set; }
    }
}
