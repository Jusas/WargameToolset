using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitExtractor.DataModels
{
    internal class SpecializationVeterancyBonus
    {
        [JsonIgnore]
        public string SpecializationNameLocalizationHash { get; set; }
        [JsonProperty("Specialization")]
        public string SpecializationName { get; set; }
        public Dictionary<string, int> Bonuses { get; set; }
    }
}
