using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitExtractor.DataModels
{
    public class DeckModifierSet
    {
        public int ActivationPoints { get; set; }
        public Dictionary<int, int> Availability { get; set; }
        public Dictionary<int, int> ActivationCost { get; set; }
        public Dictionary<int, int> SlotsAvailable { get; set; }
        public Dictionary<int, int> ExperienceBonus { get; set; }
    }
}
