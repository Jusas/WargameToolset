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
            Dictionary<string, List<object>> output = new Dictionary<string, List<object>>();

            var deckSerializer = source.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckSerializer"));
            var instance = deckSerializer.Instances.First();
            var natoUnitList = instance.PropertyValues.First(x => x.Property.Name == "OtanUnitIds");
            var pactUnitList = instance.PropertyValues.First(x => x.Property.Name == "PactUnitIds");

            var unitLists = new[] { natoUnitList, pactUnitList };

            foreach(var unitList in unitLists)
            {
                var units = new List<object>();
                output.Add(unitList.Property.Name, units);
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

        //private List<object> GatherNationalAvailabilityBonuses(Configuration configuration, DataSource dataSource)
        //{
        //    var bonuses = new List<NationCoalitionAvailabilityBonus>();

        //    var ndfBinary = GetDataSource<NdfBinary>(configuration.DataMappings["DeckBonusData"]);

        //    var deckRuleClass = ndfBinary.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckRuleManager"));
        //    var deckRules = deckRuleClass.Instances.First();
        //    var countryModifiersCollection =
        //        deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForCountry").Value as NdfMapList;
        //    var coalitionModifiersCollection =
        //        deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForCoalition").Value as NdfMapList;

        //    var modifierCollections = new[] { countryModifiersCollection, coalitionModifiersCollection };

        //    foreach (var modifierCollection in modifierCollections)
        //    {

        //        foreach (var mods in modifierCollection)
        //        {
        //            var bonus = new NationCoalitionAvailabilityBonus() { Bonuses = new Dictionary<string, int>() };
        //            bonuses.Add(bonus);

        //            var a = mods.Value as NdfMap;
        //            var key = a.Key.Value as NdfString;
        //            bonus.NationCoalition = key.ToString();

        //            var val = a.Value as MapValueHolder;
        //            var objRef = val.Value as NdfObjectReference;
        //            var availability = objRef.Instance.PropertyValues.First(pv => pv.Property.Name == "Availability").Value as NdfMapList;

        //            foreach (var item in availability)
        //            {
        //                var bonusEntry = item.Value as NdfMap;
        //                var v = bonusEntry.Key.Value as NdfInt32;
        //                var bonusUnitCategoryName = configuration.UnitCategories.FirstOrDefault(x => x.Value == v.Value.ToString()).Name;

        //                var bonusValue = bonusEntry.Value as MapValueHolder;
        //                v = bonusValue.Value as NdfInt32;
        //                var bonusUnitAvailabilityBoost = (int)v.Value;

        //                bonus.Bonuses.Add(bonusUnitCategoryName, bonusUnitAvailabilityBoost);
        //            }

        //        }
        //    }

        //    return bonuses;

        //}

        //private List<object> GatherDeckSpecVeterancyBonuses(Configuration configuration, DataSource dataSource)
        //{
        //    var veterancyBonuses = new List<SpecializationVeterancyBonus>();

        //    var ndfBinary = GetDataSource<NdfBinary>(configuration.DataMappings["DeckBonusData"]);

        //    var deckRuleClass = ndfBinary.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckRuleManager"));
        //    var deckRules = deckRuleClass.Instances.First();

        //    var deckSpecTypeModifiersCollection = deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForUnitType").Value as NdfMapList;

        //    foreach (var mods in deckSpecTypeModifiersCollection)
        //    {
        //        var bonus = new SpecializationVeterancyBonus() { Bonuses = new Dictionary<string, int>() };
        //        veterancyBonuses.Add(bonus);

        //        var a = mods.Value as NdfMap;
        //        var key = a.Key.Value as NdfLocalisationHash;
        //        bonus.SpecializationNameLocalizationHash = key.ToString();

        //        var val = a.Value as MapValueHolder;
        //        var objRef = val.Value as NdfObjectReference;
        //        var experience = objRef.Instance.PropertyValues.First(pv => pv.Property.Name == "ExperienceBonus").Value as NdfMapList;

        //        if (experience != null)
        //        {
        //            foreach (var item in experience)
        //            {
        //                var bonusEntry = item.Value as NdfMap;
        //                var v = bonusEntry.Key.Value as NdfInt32;
        //                var bonusUnitCategoryName = configuration.UnitCategories.FirstOrDefault(x => x.Value == v.Value.ToString()).Name;

        //                var bonusValue = bonusEntry.Value as MapValueHolder;
        //                v = bonusValue.Value as NdfInt32;
        //                var bonusUnitVetBoost = (int)v.Value;

        //                bonus.Bonuses.Add(bonusUnitCategoryName, bonusUnitVetBoost);
        //            }
        //        }

        //    }

        //    var tm = GetDataSource<TradManager>(configuration.DataMappings["LocalizationOutgame"]);

        //    foreach (var bonus in veterancyBonuses)
        //    {
        //        bonus.SpecializationName = tm.Entries.FirstOrDefault(e => e.HashView == bonus.SpecializationNameLocalizationHash).Content;
        //        bonus.SpecializationName = bonus.SpecializationName.Substring(bonus.SpecializationName.LastIndexOf(' ') + 1); // "#Mecanized Mechanized", we want the last part.
        //    }

        //    return veterancyBonuses;
        //}

        //private List<object> GatherUnits(Configuration configuration, DataSource dataSource)
        //{
        //    List<FactionUnitMappings> factionUnitLists = new List<FactionUnitMappings>();

        //    var ndfBinary = GetDataSource<NdfBinary>(configuration.DataMappings["UnitData"]);

        //    var unitNamesClass = ndfBinary.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckSerializer"));


        //    foreach (var factionUnitList in configuration.DeckroomSerializerUnitLists)
        //    {
        //        var factionUnitIds = unitNamesClass.Instances.First().PropertyValues.Where(
        //                pv => pv.Property.Name.Equals(factionUnitList.Source, StringComparison.InvariantCultureIgnoreCase)).First();
        //        var mappings = factionUnitIds.Value as NdfMapList;
        //        factionUnitLists.Add(new FactionUnitMappings { Mappings = mappings, FactionName = factionUnitList.FactionName });
        //    }

        //    foreach (var unitList in factionUnitLists)
        //    {
        //        foreach (var mapping in unitList.Mappings)
        //        {
        //            Unit unit = new Unit();

        //            var map = mapping.Value as NdfMap;
        //            var v = map.Key.Value as NdfUInt32;
        //            uint unitDeckId = (uint)v.Value;

        //            var mapValue = map.Value as MapValueHolder;
        //            var unitRef = mapValue.Value as NdfObjectReference;
        //            var unitInst = unitRef.Instance;

        //            unit.Id = unitInst.Id;
        //            unit.DeckId = unitDeckId;
        //            unit.UnitNameToken = unitInst.PropertyValues.First(pv => pv.Property.Name == "NameInMenuToken").Value.ToString();
        //            //unit.UnitCategory = configuration.UnitCategories.FirstOrDefault(
        //            //    x => x.Value == unitInst.PropertyValues.First(pv => pv.Property.Name == "Factory").Value.ToString())?.Name;
        //            var unitCat = unitInst.PropertyValues.First(pv => pv.Property.Name == "Factory").Value;
        //            if (unitCat is NdfNull)
        //                unit.Factory = 0;
        //            else
        //                unit.Factory = BitConverter.ToInt32(unitCat.GetBytes(), 0);


        //            unit.Nationality =
        //                unitInst.PropertyValues.First(pv => pv.Property.Name == "MotherCountry").Value.ToString();
        //            //unit.Faction = unitList.FactionName;

        //            var maxDeployableAmount =
        //                unitInst.PropertyValues.First(pv => pv.Property.Name == "MaxDeployableAmount").Value;
        //            if (maxDeployableAmount is NdfNull)
        //                unit.MaxDeployableAmount = new[] { 0, 0, 0, 0, 0 };
        //            else
        //                unit.MaxDeployableAmount =
        //            unit.MaxDeployableAmount = (maxDeployableAmount as NdfCollection).Select(a => BitConverter.ToInt32(a.Value.GetBytes(), 0)).ToArray();


        //            var productionPrice =
        //                unitInst.PropertyValues.First(pv => pv.Property.Name == "ProductionPrice").Value;
        //            if (productionPrice is NdfNull)
        //                unit.ProductionPrice = 0;
        //            else
        //                unit.ProductionPrice = (productionPrice as NdfCollection).Select(a => BitConverter.ToInt32(a.Value.GetBytes(), 0)).First();

        //            var maxpacks = unitInst.PropertyValues.First(pv => pv.Property.Name == "MaxPacks").Value;
        //            if (maxpacks is NdfNull)
        //                unit.MaxPacks = 0;
        //            else
        //                unit.MaxPacks = BitConverter.ToInt32(maxpacks.GetBytes(), 0);

        //            var icontypeprop = unitInst.PropertyValues.FirstOrDefault(pv => pv.Property.Name == "IconeType");
        //            if (icontypeprop == null)
        //                unit.IconType = -1;
        //            else
        //            {
        //                var icontype = unitInst.PropertyValues.First(pv => pv.Property.Name == "IconeType").Value;
        //                if (icontype is NdfNull)
        //                    unit.IconType = 0;
        //                else
        //                    unit.IconType = BitConverter.ToInt32(icontype.GetBytes(), 0);
        //            }

        //            unitList.Units.Add(unit);
        //        }
        //    }

        //    var tm = GetDataSource<TradManager>(configuration.DataMappings["LocalizationUnits"]);

        //    foreach (var unitList in factionUnitLists)
        //    {
        //        foreach(var unit in unitList.Units)
        //        {
        //            var actualUnitName = tm.Entries.FirstOrDefault(e => e.HashView == unit.UnitNameToken).Content;
        //            if (!string.IsNullOrEmpty(actualUnitName))
        //                unit.Name = actualUnitName;
        //        }
        //    }

        //    return factionUnitLists;
        //}
    }
}
