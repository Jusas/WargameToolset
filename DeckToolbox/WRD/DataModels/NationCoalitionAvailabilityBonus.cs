using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitExtractor.DataModels
{
    [Obsolete]
    public class NationCoalitionAvailabilityBonus
    {
        public string NationCoalitionEugenString { get; set; }
        public Dictionary<string, int> Bonuses { get; set; }
    }
}
