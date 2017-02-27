using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DeckToolbox.Utils;
using DeckToolbox.WRD.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeckToolbox.WRD.Resolvers
{
    public class JsonFileSourceUnitResolver : IUnitResolver
    {
        public IList<string> JsonSourceFiles { get; set; }
        private Dictionary<string, JToken> _dataSources = new Dictionary<string, JToken>();
        private IJsonFileReader _jsonFileReader;

        public JsonFileSourceUnitResolver(IJsonFileReader jsonReader)
        {
            JsonSourceFiles = new List<string>();
            _jsonFileReader = jsonReader;
        }

        public void Initialize()
        {
            foreach (var file in JsonSourceFiles)
            {
                try
                {
                    var txt = _jsonFileReader.ReadFile(file);
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
        
        public Unit GetUnitData(int unitId)
        {
            var source = _dataSources["UnitData"];
            var allUnits = source.Children().SelectMany(x => x.Children().SelectMany(y => y)).ToArray();
            var unitJtoken = allUnits.FirstOrDefault(x => x["Id"].ToString() == unitId.ToString());
            var unitData = unitJtoken?.ToObject<Unit>();
            return unitData;
        }

        public Unit GetUnitData(int factionId, int unitDeckId)
        {
            var source = _dataSources["UnitData"];
            source = factionId == 0 ? source["OtanUnitIds"] : source["PactUnitIds"];
            var unitJtoken = source.FirstOrDefault(x => x["DeckId"].ToString() == unitDeckId.ToString());
            var unitData = unitJtoken?.ToObject<Unit>();
            return unitData;
        }
    }
}
