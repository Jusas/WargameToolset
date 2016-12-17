using moddingSuite.BL;
using moddingSuite.BL.Ndf;
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
    internal class UnitDataExtractor
    {

        private class FactionUnitMappings
        {
            public string FactionName { get; set; }
            public NdfMapList Mappings;
            public List<Unit> Units { get; set; } = new List<Unit>();
        }
        
        public void ExtractUsingConfiguration(Configuration configuration)
        {

            var factionUnitLists = GatherUnits(configuration);
            var natAvailBonuses = GatherNationalAvailabilityBonuses(configuration);
            var deckSpecVetBonuses = GatherDeckSpecVeterancyBonuses(configuration);
            // todo also output unit categories, from configuration
            // todo also include comments output "This file was generated from game data files"

            // todo also output specializations from deckSpecVetBonuses.SpecializationName - or is it needed, all specs are actually listed in the vet file...

            File.WriteAllText("wrd-unit-list.json", JsonConvert.SerializeObject(factionUnitLists.SelectMany(x => x.Units), Formatting.Indented));
            File.WriteAllText("wrd-nat-avail-bonuses.json", JsonConvert.SerializeObject(natAvailBonuses, Formatting.Indented));
            File.WriteAllText("wrd-deck-spec-vet-bonuses.json", JsonConvert.SerializeObject(deckSpecVetBonuses, Formatting.Indented));
        }

        private List<NationCoalitionAvailabilityBonus> GatherNationalAvailabilityBonuses(Configuration configuration)
        {
            var bonuses = new List<NationCoalitionAvailabilityBonus>();

            EdataManager eManager = new EdataManager(configuration.NationCoalitionAvailabilityBonusDataFile);
            eManager.ParseEdataFile();
            var files = eManager.Files;
            var everything = eManager.Files.FirstOrDefault(f => f.Path.Contains("everything.ndfbin"));
            var ndfbinReader = new NdfbinReader();
            NdfBinary ndfBinary = ndfbinReader.Read(eManager.GetRawData(everything));

            var deckRuleClass = ndfBinary.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckRuleManager"));
            var deckRules = deckRuleClass.Instances.First();
            var countryModifiersCollection =
                deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForCountry").Value as NdfMapList;
            var coalitionModifiersCollection =
                deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForCoalition").Value as NdfMapList;
            
            var modifierCollections = new[] { countryModifiersCollection, coalitionModifiersCollection };

            foreach (var modifierCollection in modifierCollections)
            {

                foreach (var mods in modifierCollection)
                {
                    var bonus = new NationCoalitionAvailabilityBonus() { Bonuses = new Dictionary<string, int>() };
                    bonuses.Add(bonus);

                    var a = mods.Value as NdfMap;
                    var key = a.Key.Value as NdfString;
                    bonus.NationCoalition = key.ToString();

                    var val = a.Value as MapValueHolder;
                    var objRef = val.Value as NdfObjectReference;
                    var availability = objRef.Instance.PropertyValues.First(pv => pv.Property.Name == "Availability").Value as NdfMapList;

                    foreach (var item in availability)
                    {
                        var bonusEntry = item.Value as NdfMap;
                        var v = bonusEntry.Key.Value as NdfInt32;
                        var bonusUnitCategoryName = configuration.UnitCategories.FirstOrDefault(x => x.Value == v.Value.ToString()).Name;

                        var bonusValue = bonusEntry.Value as MapValueHolder;
                        v = bonusValue.Value as NdfInt32;
                        var bonusUnitAvailabilityBoost = (int)v.Value;

                        bonus.Bonuses.Add(bonusUnitCategoryName, bonusUnitAvailabilityBoost);
                    }

                }
            }

            return bonuses;

        }

        private List<SpecializationVeterancyBonus> GatherDeckSpecVeterancyBonuses(Configuration configuration)
        {
            var veterancyBonuses = new List<SpecializationVeterancyBonus>();

            EdataManager eManager = new EdataManager(configuration.DeckSpecVeterancyBonusDataFile);
            eManager.ParseEdataFile();
            var files = eManager.Files;
            var everything = eManager.Files.FirstOrDefault(f => f.Path.Contains("everything.ndfbin"));
            var ndfbinReader = new NdfbinReader();
            NdfBinary ndfBinary = ndfbinReader.Read(eManager.GetRawData(everything));

            var deckRuleClass = ndfBinary.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckRuleManager"));
            var deckRules = deckRuleClass.Instances.First();
            
            var deckSpecTypeModifiersCollection = deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForUnitType").Value as NdfMapList;

            foreach (var mods in deckSpecTypeModifiersCollection)
            {
                var bonus = new SpecializationVeterancyBonus() { Bonuses = new Dictionary<string, int>() };
                veterancyBonuses.Add(bonus);

                var a = mods.Value as NdfMap;
                var key = a.Key.Value as NdfLocalisationHash;
                bonus.SpecializationNameLocalizationHash = key.ToString();

                var val = a.Value as MapValueHolder;
                var objRef = val.Value as NdfObjectReference;
                var experience = objRef.Instance.PropertyValues.First(pv => pv.Property.Name == "ExperienceBonus").Value as NdfMapList;

                if (experience != null)
                {
                    foreach (var item in experience)
                    {
                        var bonusEntry = item.Value as NdfMap;
                        var v = bonusEntry.Key.Value as NdfInt32;
                        var bonusUnitCategoryName = configuration.UnitCategories.FirstOrDefault(x => x.Value == v.Value.ToString()).Name;

                        var bonusValue = bonusEntry.Value as MapValueHolder;
                        v = bonusValue.Value as NdfInt32;
                        var bonusUnitVetBoost = (int)v.Value;

                        bonus.Bonuses.Add(bonusUnitCategoryName, bonusUnitVetBoost);
                    }
                }

            }
            
            eManager = new EdataManager(configuration.LocalizationsDataFile);
            eManager.ParseEdataFile();

            var interface_outgame = eManager.Files.First(f => f.Path.Contains(@"us\localisation\interface_outgame.dic"));
            var tm = new TradManager(eManager.GetRawData(interface_outgame));

            foreach (var bonus in veterancyBonuses)
            {
                bonus.SpecializationName = tm.Entries.FirstOrDefault(e => e.HashView == bonus.SpecializationNameLocalizationHash).Content;
                bonus.SpecializationName = bonus.SpecializationName.Substring(bonus.SpecializationName.LastIndexOf(' ') + 1); // "#Mecanized Mechanized", we want the last part.
            }

            return veterancyBonuses;
        }

        private List<FactionUnitMappings> GatherUnits(Configuration configuration)
        {
            List<FactionUnitMappings> factionUnitLists = new List<FactionUnitMappings>();

            EdataManager eManager = new EdataManager(configuration.UnitDataFile);
            eManager.ParseEdataFile();

            var files = eManager.Files;
            var everything = eManager.Files.FirstOrDefault(f => f.Path.Contains("everything.ndfbin"));
            var ndfbinReader = new NdfbinReader();

            NdfBinary ndfBinary = ndfbinReader.Read(eManager.GetRawData(everything));
            var unitNamesClass = ndfBinary.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckSerializer"));

            
            foreach (var factionUnitList in configuration.DeckroomSerializerUnitLists)
            {
                var factionUnitIds = unitNamesClass.Instances.First().PropertyValues.Where(
                        pv => pv.Property.Name.Equals(factionUnitList.Source, StringComparison.InvariantCultureIgnoreCase)).First();
                var mappings = factionUnitIds.Value as NdfMapList;
                factionUnitLists.Add(new FactionUnitMappings { Mappings = mappings, FactionName = factionUnitList.FactionName });
            }

            foreach (var unitList in factionUnitLists)
            {
                foreach (var mapping in unitList.Mappings)
                {
                    Unit unit = new Unit();

                    var map = mapping.Value as NdfMap;
                    var v = map.Key.Value as NdfUInt32;
                    uint unitDeckId = (uint)v.Value;

                    var mapValue = map.Value as MapValueHolder;
                    var unitRef = mapValue.Value as NdfObjectReference;
                    var unitInst = unitRef.Instance;

                    unit.Id = unitInst.Id;
                    unit.DeckId = unitDeckId;
                    unit.UnitNameToken = unitInst.PropertyValues.First(pv => pv.Property.Name == "NameInMenuToken").Value.ToString();
                    //unit.UnitCategory = configuration.UnitCategories.FirstOrDefault(
                    //    x => x.Value == unitInst.PropertyValues.First(pv => pv.Property.Name == "Factory").Value.ToString())?.Name;
                    var unitCat = unitInst.PropertyValues.First(pv => pv.Property.Name == "Factory").Value;
                    if (unitCat is NdfNull)
                        unit.Factory = 0;
                    else
                        unit.Factory = BitConverter.ToInt32(unitCat.GetBytes(), 0);
                    

                    unit.Nationality =
                        unitInst.PropertyValues.First(pv => pv.Property.Name == "MotherCountry").Value.ToString();
                    //unit.Faction = unitList.FactionName;

                    var maxDeployableAmount =
                        unitInst.PropertyValues.First(pv => pv.Property.Name == "MaxDeployableAmount").Value;
                    if (maxDeployableAmount is NdfNull)
                        unit.MaxDeployableAmount = new[] { 0, 0, 0, 0, 0 };
                    else
                        unit.MaxDeployableAmount =
                    unit.MaxDeployableAmount = (maxDeployableAmount as NdfCollection).Select(a => BitConverter.ToInt32(a.Value.GetBytes(), 0)).ToArray();


                    var productionPrice =
                        unitInst.PropertyValues.First(pv => pv.Property.Name == "ProductionPrice").Value;
                    if (productionPrice is NdfNull)
                        unit.ProductionPrice = 0;
                    else
                        unit.ProductionPrice = (productionPrice as NdfCollection).Select(a => BitConverter.ToInt32(a.Value.GetBytes(), 0)).First();

                    var maxpacks = unitInst.PropertyValues.First(pv => pv.Property.Name == "MaxPacks").Value;
                    if (maxpacks is NdfNull)
                        unit.MaxPacks = 0;
                    else
                        unit.MaxPacks = BitConverter.ToInt32(maxpacks.GetBytes(), 0);

                    var icontypeprop = unitInst.PropertyValues.FirstOrDefault(pv => pv.Property.Name == "IconeType");
                    if (icontypeprop == null)
                        unit.IconType = -1;
                    else
                    {
                        var icontype = unitInst.PropertyValues.First(pv => pv.Property.Name == "IconeType").Value;
                        if (icontype is NdfNull)
                            unit.IconType = 0;
                        else
                            unit.IconType = BitConverter.ToInt32(icontype.GetBytes(), 0);
                    }

                    unitList.Units.Add(unit);
                }
            }

            eManager = new EdataManager(configuration.UnitNamesDataFile);
            eManager.ParseEdataFile();
            files = eManager.Files;

            var enLocalizedUnitnames = eManager.Files.FirstOrDefault(f => f.Path.Contains(@"us\localisation\unites.dic"));
            TradManager tm = new TradManager(eManager.GetRawData(enLocalizedUnitnames));

            foreach (var unitList in factionUnitLists)
            {
                foreach(var unit in unitList.Units)
                {
                    var actualUnitName = tm.Entries.FirstOrDefault(e => e.HashView == unit.UnitNameToken).Content;
                    if (!string.IsNullOrEmpty(actualUnitName))
                        unit.Name = actualUnitName;
                }
            }

            return factionUnitLists;
        }
    }
}
