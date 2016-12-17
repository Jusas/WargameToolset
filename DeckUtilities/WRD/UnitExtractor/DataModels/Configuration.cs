using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitExtractor.DataModels
{
    public class Configuration
    {
        public class UnitCategory
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class DeckroomSerializerUnitList
        {
            public string FactionName { get; set; }
            public string Source { get; set; }
        }

        public string UnitDataFile { get; set; }
        public string LocalizationsDataFile { get; set; }
        public string UnitNamesDataFile { get; set; }
        public string OutputDirectory { get; set; }
        public string NationCoalitionAvailabilityBonusDataFile { get; set; }
        public string DeckSpecVeterancyBonusDataFile { get; set; }

        public List<UnitCategory> UnitCategories { get; set; }
        public List<DeckroomSerializerUnitList> DeckroomSerializerUnitLists { get; set; }
    }
}
