using moddingSuite.Model.Ndfbin;
using moddingSuite.Model.Ndfbin.Types.AllTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitExtractor.DataModels;

namespace UnitExtractor
{
    public static class DeckData
    {
        /// <summary>
        /// Gets unit categories (or 'eras', ie. CatA, CatB, CatC).
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static object GetUnitCategories(Configuration configuration, DataSource dataSource)
        {
            var source = dataSource.GetDataSource<NdfBinary>(configuration.DataMappings["DeckSerializationData"]);
            List<object> output = new List<object>();

            var deckAttributes = source.Classes.FirstOrDefault(c => c.Name == "TDeckAttributes");
            var instance = deckAttributes.Instances.First();
            var eraMap = instance.PropertyValues.First(x => x.Property.Name == "MapUnitCategoryToDate").Value as NdfMapList;
            for (var i = 0; i < eraMap.Count; i++)
            {
                var keyValueMap = eraMap[i].Value as NdfMap;
                var key = keyValueMap.Key.Value as NdfLocalisationHash;
                var value = ((MapValueHolder)keyValueMap.Value).Value as NdfUInt32;

                output.Add(new
                {
                    Id = i,
                    LocalizationId = key.ToString(),
                    Name = dataSource.GetLocalizedString(key.ToString(), configuration.DataMappings["LocalizationOutgame"]),
                    Year = (UInt32)value.Value
                });
            }

            return output;
        }

        public static object GetNationalities(Configuration configuration, DataSource dataSource)
        {
            var source = dataSource.GetDataSource<NdfBinary>(configuration.DataMappings["DeckSerializationData"]);
            List<object> output = new List<object>();

            var deckSerializer = source.Classes.FirstOrDefault(c => c.Name == "TShowRoomDeckSerializer");
            var instance = deckSerializer.Instances.First();
            var nationalities = instance.PropertyValues.First(x => x.Property.Name == "Nationalities").Value as NdfCollection;
            for (var i = 0; i < nationalities.Count; i++)
            {
                var nationality = nationalities[i].Value as NdfInt32;
                
                output.Add(new
                {
                    Id = i,
                    Nationality = (int)nationality.Value,
                    // a little hardcoded junk, not sure where to nicely get the corresponding values.
                    Name = i == 0 ? "BLUFOR" : i == 1 ? "REDFOR" : ""
                });
            }

            return output;
        }

        public static object GetCountries(Configuration configuration, DataSource dataSource)
        {
            var source = dataSource.GetDataSource<NdfBinary>(configuration.DataMappings["DeckSerializationData"]);
            Dictionary<string, List<object>> output = new Dictionary<string, List<object>>();

            var deckSerializer = source.Classes.FirstOrDefault(c => c.Name == "TShowRoomDeckSerializer");
            var instance = deckSerializer.Instances.First();
            var countriesNato = instance.PropertyValues.First(x => x.Property.Name == "CountriesNATO");
            var countriesPact = instance.PropertyValues.First(x => x.Property.Name == "CountriesPACT");

            var countryGroups = new[] { countriesNato, countriesPact };

            for (var i = 0; i < countryGroups.Length; i++)
            {
                var countries = new List<object>();
                output.Add(countryGroups[i].Property.Name, countries);
                var list = countryGroups[i].Value as NdfCollection;

                for(var j = 0; j < list.Count; j++)
                {
                    var country = list[j].Value.ToString();
                    var id = j;

                    countries.Add(new
                    {
                        Id = id,
                        Name = country
                    });
                }
            }

            return output;
        }

        public static object GetCoalitions(Configuration configuration, DataSource dataSource)
        {
            var source = dataSource.GetDataSource<NdfBinary>(configuration.DataMappings["DeckSerializationData"]);
            List<object> output = new List<object>();

            var deckSerializer = source.Classes.FirstOrDefault(c => c.Name == "TShowRoomDeckSerializer");
            var instance = deckSerializer.Instances.First();
            var coalitions = instance.PropertyValues.First(x => x.Property.Name == "Coalitions").Value as NdfCollection;
            
            for (var i = 0; i < coalitions.Count; i++)
            {
                var coalition = coalitions[i].Value.ToString();
                output.Add(new
                {
                    Id = i,
                    Name = coalition
                });
            }

            return output;
        }

        /// <summary>
        /// Unit types (or deck types), ie. Motorized, Armored, etc.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static object GetUnitTypes(Configuration configuration, DataSource dataSource)
        {
            var source = dataSource.GetDataSource<NdfBinary>(configuration.DataMappings["DeckSerializationData"]);
            List<object> output = new List<object>();

            var deckSerializer = source.Classes.FirstOrDefault(c => c.Name == "TShowRoomDeckSerializer");
            var instance = deckSerializer.Instances.First();
            var unitTypes = instance.PropertyValues.First(x => x.Property.Name == "UnitTypes").Value as NdfCollection;

            for (var i = 0; i < unitTypes.Count; i++)
            {
                var ut = unitTypes[i].Value as NdfLocalisationHash;
                output.Add(new
                {
                    Id = i,
                    LocalizationId = ut.ToString(),
                    Name = dataSource.GetLocalizedString(ut.ToString(), configuration.DataMappings["LocalizationOutgame"])
                });
            }

            return output;
        }

        /// <summary>
        /// I couldn't find this data, ie. the ID numbers correlation to unit types.
        /// So I'm just monkeying this up, until the data gets found.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static object GetFactories(Configuration configuration, DataSource dataSource)
        {
            return new List<object>
            {
                new
                {
                    Id = 3,
                    Name = "Logistics"
                },
                new
                {
                    Id = 10,
                    Name = "Recon"
                },
                new
                {
                    Id = 9,
                    Name = "Armor"
                },
                new
                {
                    Id = 6,
                    Name = "Infantry"
                },
                new
                {
                    Id = 13,
                    Name = "Support"
                },
                new
                {
                    Id = 11,
                    Name = "Helicopter"
                },
                new
                {
                    Id = 7,
                    Name = "Plane"
                },
                new
                {
                    Id = 12,
                    Name = "Navy"
                }
            };
        }

        public static object GetDeckModifiers(Configuration configuration, DataSource dataSource)
        {
            var source = dataSource.GetDataSource<NdfBinary>(configuration.DataMappings["DeckSerializationData"]);
            Dictionary<string, List<object>> output = new Dictionary<string, List<object>>();

            var deckSerializer = source.Classes.FirstOrDefault(c => c.Name == "TShowRoomDeckRuleManager");
            var instance = deckSerializer.Instances.First();
            List<NdfPropertyValue> allModifiers = new List<NdfPropertyValue>();

            allModifiers.Add(instance.PropertyValues.First(x => x.Property.Name == "ModifiersForCountry"));
            allModifiers.Add(instance.PropertyValues.First(x => x.Property.Name == "ModifiersForCoalition"));
            allModifiers.Add(instance.PropertyValues.First(x => x.Property.Name == "ModifiersForAlliance"));
            allModifiers.Add(instance.PropertyValues.First(x => x.Property.Name == "ModifiersForUnitType"));
            allModifiers.Add(instance.PropertyValues.First(x => x.Property.Name == "ModifiersForUnitCategory"));


            for (var i = 0; i < allModifiers.Count; i++)
            {
                var modifiers = new List<object>();
                output.Add(allModifiers[i].Property.Name, modifiers);
                var list = allModifiers[i].Value as NdfMapList;

                for (var j = 0; j < list.Count; j++)
                {
                    var modifier = list[j].Value as NdfMap;
                    var modifierKey = modifier.Key.Value.ToString();
                    var modifierValue = (MapValueHolder)modifier.Value;

                    var deckModifierSet = new DeckModifierSet();
                    var modifierEntry = new
                    {
                        GrantedTo = modifierKey,
                        Modifiers = deckModifierSet
                    };
                    modifiers.Add(modifierEntry);

                    var objRef = modifierValue.Value as NdfObjectReference;
                    var obj = objRef.Instance;

                    deckModifierSet.ActivationPoints = (int)obj.GetInstancePropertyValue<int>("ActivationPoints");
                    deckModifierSet.Availability = (Dictionary<int, int>)obj.GetInstancePropertyValue<Dictionary<int, int>>("Availability");
                    deckModifierSet.ActivationCost = (Dictionary<int, int>)obj.GetInstancePropertyValue<Dictionary<int, int>>("ActivationCost");
                    deckModifierSet.SlotsAvailable = (Dictionary<int, int>)obj.GetInstancePropertyValue<Dictionary<int, int>>("SlotsAvailable");
                    deckModifierSet.ExperienceBonus = (Dictionary<int, int>)obj.GetInstancePropertyValue<Dictionary<int, int>>("ExperienceBonus");
                    
                }
            }

            return output;
        }


        public static object GetFactionMappings(Configuration configuration, DataSource dataSource)
        {
            var source = dataSource.GetDataSource<NdfBinary>(configuration.DataMappings["DeckSerializationData"]);
            Dictionary<string, List<object>> output = new Dictionary<string, List<object>>();

            var deckAttributes = source.Classes.FirstOrDefault(c => c.Name == "TDeckAttributes");
            var instance = deckAttributes.Instances.First();
            List<NdfPropertyValue> allMappings = new List<NdfPropertyValue>();

            allMappings.Add(instance.PropertyValues.First(x => x.Property.Name == "MapFactionToCountry"));
            allMappings.Add(instance.PropertyValues.First(x => x.Property.Name == "MapFactionToCoalition"));
            allMappings.Add(instance.PropertyValues.First(x => x.Property.Name == "MapCoalitionToCountry"));
            
            for (var i = 0; i < allMappings.Count; i++)
            {
                var mappings = new List<object>();
                output.Add(allMappings[i].Property.Name, mappings);
                var list = allMappings[i].Value as NdfMapList;

                for (var j = 0; j < list.Count; j++)
                {
                    var mapping = list[j].Value as NdfMap;
                    var mappingKey = mapping.Key.Value.ToString();
                    var mappingValue = ((MapValueHolder)mapping.Value).Value as NdfCollection;

                    var mappingEntry = new
                    {
                        Key = mappingKey,
                        Values = new List<string>()
                    };
                    foreach(var collectionEntry in mappingValue)
                    {
                        mappingEntry.Values.Add(collectionEntry.Value.ToString());
                    }

                    mappings.Add(mappingEntry);
                }
            }

            return output;
        }


    }
}
