using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeckToolbox.WRD.Resolvers
{
    public class JsonFileSourceDeckValueResolver : IDeckValueResolver
    {

        public IList<string> JsonSourceFiles { get; set; }
        private Dictionary<string, JToken> _dataSources = new Dictionary<string, JToken>();

        public JsonFileSourceDeckValueResolver()
        {
            JsonSourceFiles = new List<string>();
        }

        public void Initialize()
        {
            foreach (var file in JsonSourceFiles)
            {
                try
                {
                    var txt = File.ReadAllText(file);
                    var dataSource = JToken.Parse(txt);
                    var id = dataSource["MetaData"]["DataIdentifier"].ToString();
                    var data = dataSource["Data"];
                    _dataSources.Add(id, data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        }

        public string GetFactionName(int factionId)
        {
            var source = _dataSources["UnitNationalities"];
            var name = source.FirstOrDefault(x => x["Nationality"].ToString() == factionId.ToString())?["Name"].ToString();
            return name;
        }

        public string GetCountryName(int factionId, int countryDeckId)
        {
            // The mapping from faction 0 => blue/CountriesNATO, 1 => red/CountriesPACT isn't explicitly anywhere.
            // Fragile assumptions are being made and could be broken by future patches (extremely unlikely though).
            var source = _dataSources["UnitCountries"];
            var sourceList = factionId == 0 ? source["CountriesNATO"] : source["CountriesPACT"];            
            var name = sourceList.FirstOrDefault(x => x["Id"].ToString() == countryDeckId.ToString())?["Name"].ToString();
            return name;
        }

        public string GetCoalitionName(int coalitionDeckId)
        {
            var source = _dataSources["UnitCoalitions"];
            var name = source.FirstOrDefault(x => x["Id"].ToString() == coalitionDeckId.ToString())?["Name"].ToString();
            return name;
        }

        public string GetDeckSpecializationName(int unitTypeId)
        {
            var source = _dataSources["UnitTypes"];
            var name = source.FirstOrDefault(x => x["Id"].ToString() == unitTypeId.ToString())?["Name"].ToString();
            return name;
        }

        public string GetDeckEraName(int categoryId)
        {
            var source = _dataSources["UnitCategories"];
            var name = source.FirstOrDefault(x => x["Id"].ToString() == categoryId.ToString())?["Name"].ToString();
            return name;
        }

        public string GetFactoryName(int factory)
        {
            var source = _dataSources["UnitFactories"];
            var name = source.FirstOrDefault(x => x["Id"].ToString() == factory.ToString())?["Name"].ToString();
            return name;
        }
    }
}
