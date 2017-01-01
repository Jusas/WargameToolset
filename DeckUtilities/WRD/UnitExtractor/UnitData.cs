using moddingSuite.BL;
using moddingSuite.BL.Ndf;
using moddingSuite.Model.Edata;
using moddingSuite.Model.Ndfbin;
using moddingSuite.Model.Ndfbin.Types.AllTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitExtractor.DataModels;

namespace UnitExtractor
{
    public static class UnitData
    {
        public static object GetUnits(Configuration configuration, DataSource dataSource)
        {
            var source = dataSource.GetDataSource<NdfBinary>(configuration.DataMappings["UnitData"]);
            Dictionary<string, List<object>> outputData = new Dictionary<string, List<object>>();
            var output = new ExportedDataTable()
            {
                MetaData = new ExportedDataTable.MetaDataModel()
                {
                    DataIdentifier = "UnitData",
                    ExtractedFrom = new[]
                    {
                        configuration.DataMappings["UnitData"]
                    }
                },
                Data = outputData
            };

            var deckSerializer = source.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckSerializer"));
            var instance = deckSerializer.Instances.First();
            var natoUnitList = instance.PropertyValues.First(x => x.Property.Name == "OtanUnitIds");
            var pactUnitList = instance.PropertyValues.First(x => x.Property.Name == "PactUnitIds");

            var unitLists = new[] { natoUnitList, pactUnitList };

            foreach(var unitList in unitLists)
            {
                var units = new List<object>();
                outputData.Add(unitList.Property.Name, units);
                var unitMap = unitList.Value as NdfMapList;

                for(var i = 0; i < unitMap.Count; i++)
                {
                    var map = unitMap[i].Value as NdfMap;
                    var v = map.Key.Value as NdfUInt32;
                    uint unitDeckId = (uint)v.Value;

                    var mapValue = map.Value as MapValueHolder;
                    var unitRef = mapValue.Value as NdfObjectReference;
                    var unitInst = unitRef.Instance;

                    var unit = new Unit();
                    units.Add(unit);
                    unit.DeckId = unitDeckId;
                    unit.Id = unitInst.Id;
                    unit.AliasName = unitInst.GetInstancePropertyValue<string>("AliasName") as string;
                    unit.Factory = (int)unitInst.GetInstancePropertyValue<int>("Factory");
                    unit.MaxDeployableAmount = (int[])unitInst.GetInstancePropertyValue<int[]>("MaxDeployableAmount");
                    unit.MaxPacks = Convert.ToInt32((UInt32)unitInst.GetInstancePropertyValue<UInt32>("MaxPacks"));
                    unit.MotherCountry = unitInst.GetInstancePropertyValue<string>("MotherCountry") as string;
                    unit.ProductionPrice = ((int[])unitInst.GetInstancePropertyValue<int[]>("ProductionPrice")).First();
                    unit.Name = dataSource.GetLocalizedString(unitInst.GetInstancePropertyValue<string>("NameInMenuToken") as string, configuration.DataMappings["LocalizationUnits"]);
                }
            }

            return output;
            
        }
    }
}
