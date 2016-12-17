using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using moddingSuite.BL;
using moddingSuite.BL.Ndf;
using moddingSuite.Model.Ndfbin;
using moddingSuite.Model.Ndfbin.Types.AllTypes;
using Newtonsoft.Json;
using UnitExtractor.DataModels;

namespace UnitExtractor
{
    class Program
    {

        static void Main(string[] args)
        {
            var confjson = File.ReadAllText("configuration.json");
            var conf = JsonConvert.DeserializeObject<Configuration>(confjson);
            UnitDataExtractor extractor = new UnitDataExtractor();
            extractor.ExtractUsingConfiguration(conf);
        }


        /*
        class Unit
        {
            public uint Id { get; set; }
            public string Name { get; set; }
            public uint DeckId { get; set; }
            public string UnitCategory { get; set; }
            public string Nationality { get; set; }
            public string Faction { get; set; }

            public int[] MaxDeployableAmount { get; set; }
            public int ProductionPrice { get; set; }
            public int MaxPacks { get; set; }
            public int IconType { get; set; }
        }

        class AvailabilityBonus
        {
            public string NationCoalition { get; set; }
            public Dictionary<string, int> Bonuses { get; set; }
        }

        class VeterancyBonus
        {
            public string Specialization { get; set; }
            public Dictionary<string, int> Bonuses { get; set; }
        }

        private static Dictionary<int, string> UnitCategoryMappings = new Dictionary<int, string>
        {
            {3, "Logistics"},
            {10, "Recon"},
            {9, "Armor"},
            {6, "Infantry"},
            {13, "Support"},
            {8, "Vehicle"},
            {11, "Helicopter"},
            {7, "Plane"},
            {12, "Navy"}
        };*/

            /*
        static void Main(string[] args)
        {
            string ndf_windat =
                @"H:\SteamLibrary\SteamApps\common\Wargame Red Dragon\Data\WARGAME\PC\510025133\NDF_Win.dat";
            string zz_windat_iface_outgame =
                @"H:\SteamLibrary\SteamApps\common\Wargame Red Dragon\Data\WARGAME\PC\510019512\510024196\ZZ_Win.dat";
            string zz_windat_unitnames =
                @"H:\SteamLibrary\SteamApps\common\Wargame Red Dragon\Data\WARGAME\PC\510024744\510025133\ZZ_Win.dat";

            EdataManager eManager = new EdataManager(ndf_windat);
            eManager.ParseEdataFile();
            var files = eManager.Files;
            var everything = eManager.Files.FirstOrDefault(f => f.Path.Contains("everything.ndfbin"));
            var ndfbinReader = new NdfbinReader();
            NdfBinary ndfBinary = ndfbinReader.Read(eManager.GetRawData(everything));



            Console.WriteLine(@"Collecting Unit id mappings...");
            var unitNamesClass = ndfBinary.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckSerializer"));

            var blueUnitIds = unitNamesClass.Instances.First()
                .PropertyValues.Where(
                    pv => pv.Property.Name.Equals("OtanUnitIds", StringComparison.CurrentCultureIgnoreCase)).First();
            var redUnitIds = unitNamesClass.Instances.First()
                .PropertyValues.Where(
                    pv => pv.Property.Name.Equals("PactUnitIds", StringComparison.CurrentCultureIgnoreCase)).First();

            var blueMappings = blueUnitIds.Value as NdfMapList;
            var redMappings = redUnitIds.Value as NdfMapList;

            List<Unit> blueUnits = new List<Unit>();
            List<Unit> redUnits = new List<Unit>();
            Dictionary<NdfMapList, Tuple<string, List<Unit>>> factions = new Dictionary<NdfMapList, Tuple<string, List<Unit>>>()
            {
                {blueMappings, new Tuple<string, List<Unit>>("BLUEFOR", blueUnits)},
                {redMappings, new Tuple<string, List<Unit>>("REDFOR", redUnits)}
            };
            foreach (var faction in factions)
            {
                var mappings = faction.Key;
                var unitList = faction.Value.Item2;
                var factionName = faction.Value.Item1;
                foreach (var mapping in mappings)
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
                    unit.Name = unitInst.PropertyValues.First(pv => pv.Property.Name == "NameInMenuToken").Value.ToString();
                    unit.UnitCategory =
                        UnitCategoryMappings[Int32.Parse(unitInst.PropertyValues.First(pv => pv.Property.Name == "Factory").Value.ToString())];
                    unit.Nationality =
                        unitInst.PropertyValues.First(pv => pv.Property.Name == "MotherCountry").Value.ToString();
                    unit.Faction = factionName;

                    var maxDeployableAmount =
                        unitInst.PropertyValues.First(pv => pv.Property.Name == "MaxDeployableAmount").Value;
                    if (maxDeployableAmount is NdfNull)
                        unit.MaxDeployableAmount = new[] {0, 0, 0, 0, 0};
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
                    

                    unitList.Add(unit);
                    //Console.WriteLine(mapping);
                }
            }


            //------------------------------------------------------------------

            var deckRuleClass = ndfBinary.Classes.FirstOrDefault(c => c.Name.Contains("TShowRoomDeckRuleManager"));
            var deckRules = deckRuleClass.Instances.First();
            var countryModifiersCollection =
                deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForCountry").Value as NdfMapList;
            var coalitionModifiersCollection =
                deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForCoalition").Value as NdfMapList;

            var deckSpecTypeModifiersCollection = deckRules.PropertyValues.First(pv => pv.Property.Name == "ModifiersForUnitType").Value as NdfMapList;

            var availabilityStuff = new[] {countryModifiersCollection, coalitionModifiersCollection};

            List<AvailabilityBonus> availabilityBonuses = new List<AvailabilityBonus>();

            foreach (var modifiers in availabilityStuff)
            {
                
                foreach (var mods in modifiers)
                {
                    var avBonus = new AvailabilityBonus() {Bonuses = new Dictionary<string, int>()};
                    availabilityBonuses.Add(avBonus);

                    var a = mods.Value as NdfMap;
                    var key = a.Key.Value as NdfString;
                    avBonus.NationCoalition = key.ToString();

                    var val = a.Value as MapValueHolder;
                    var objRef = val.Value as NdfObjectReference;
                    var availability = objRef.Instance.PropertyValues.First(pv => pv.Property.Name == "Availability").Value as NdfMapList;

                    foreach (var item in availability)
                    {
                        var bonusEntry = item.Value as NdfMap;
                        var v = bonusEntry.Key.Value as NdfInt32;
                        var bonusUnitCategoryName = UnitCategoryMappings[(int)v.Value];

                        var bonusValue = bonusEntry.Value as MapValueHolder;
                        v = bonusValue.Value as NdfInt32;
                        var bonusUnitAvailabilityBoost = (int) v.Value;

                        avBonus.Bonuses.Add(bonusUnitCategoryName, bonusUnitAvailabilityBoost);
                    }
                    
                }
            }

            List<VeterancyBonus> veterancyBonuses = new List<VeterancyBonus>();

            foreach (var mods in deckSpecTypeModifiersCollection)
            {
                var vetBonus = new VeterancyBonus() {Bonuses = new Dictionary<string, int>()};
                veterancyBonuses.Add(vetBonus);

                var a = mods.Value as NdfMap;
                var key = a.Key.Value as NdfLocalisationHash;
                vetBonus.Specialization = key.ToString();

                var val = a.Value as MapValueHolder;
                var objRef = val.Value as NdfObjectReference;
                var experience = objRef.Instance.PropertyValues.First(pv => pv.Property.Name == "ExperienceBonus").Value as NdfMapList;

                if (experience != null)
                {
                    foreach (var item in experience)
                    {
                        var bonusEntry = item.Value as NdfMap;
                        var v = bonusEntry.Key.Value as NdfInt32;
                        var bonusUnitCategoryName = UnitCategoryMappings[(int) v.Value];

                        var bonusValue = bonusEntry.Value as MapValueHolder;
                        v = bonusValue.Value as NdfInt32;
                        var bonusUnitVetBoost = (int) v.Value;

                        vetBonus.Bonuses.Add(bonusUnitCategoryName, bonusUnitVetBoost);
                    }
                }

            }

            
            File.WriteAllText("wrd-availability-bonuses.json", JsonConvert.SerializeObject(availabilityBonuses, Formatting.Indented));

            //------------------------------------------------------------------



            eManager = new EdataManager(zz_windat_unitnames);
            eManager.ParseEdataFile();
            files = eManager.Files;

            var usUnitnames = eManager.Files.FirstOrDefault(f => f.Path.Contains(@"us\localisation\unites.dic"));
            
            TradManager tm = new TradManager(eManager.GetRawData(usUnitnames));

            foreach (var faction in factions)
            {
                var unitList = faction.Value.Item2;
                foreach (var unit in unitList)
                {
                    var actualUnitName = tm.Entries.FirstOrDefault(e => e.HashView == unit.Name).Content;
                    if (!string.IsNullOrEmpty(actualUnitName))
                        unit.Name = actualUnitName;
                }
            }
            
            var allUnits = new List<Unit>();
            allUnits.AddRange(blueUnits);
            allUnits.AddRange(redUnits);
            File.WriteAllText("wrd-units.json", JsonConvert.SerializeObject(allUnits, Formatting.Indented));

            eManager = new EdataManager(zz_windat_iface_outgame);
            eManager.ParseEdataFile();

            var interface_outgame = eManager.Files.First(f => f.Path.Contains(@"us\localisation\interface_outgame.dic"));
            tm = new TradManager(eManager.GetRawData(interface_outgame));

            foreach (var vetBonus in veterancyBonuses)
            {
                vetBonus.Specialization = tm.Entries.FirstOrDefault(e => e.HashView == vetBonus.Specialization).Content;
                vetBonus.Specialization = vetBonus.Specialization.Substring(vetBonus.Specialization.LastIndexOf(' ')+1);
            }


            File.WriteAllText("wrd-vet-bonuses.json", JsonConvert.SerializeObject(veterancyBonuses, Formatting.Indented));

            Console.WriteLine("DONE!");


            // ----------------------------------------------------





            Console.ReadKey();
        }
        */
    }
}
