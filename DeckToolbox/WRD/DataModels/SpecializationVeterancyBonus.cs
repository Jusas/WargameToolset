using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckToolbox.WRD.DataModels
{
    public class SpecializationVeterancyBonus
    {
        public int SpecializationEugenString { get; set; }
        public Dictionary<string, int> Bonuses { get; set; }
    }
}
