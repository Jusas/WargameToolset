using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitExtractor.DataModels
{
    internal class NationCoalitionAvailabilityBonus
    {
        public string NationCoalition { get; set; }
        public Dictionary<string, int> Bonuses { get; set; }
    }
}
