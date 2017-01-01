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
            var exporter = new JsonFileExporter(Configuration.FromFile("configuration.json"), new DataSource(), @".\Json");
            exporter.AddExporter(DeckData.GetUnitCategories, "DeckCategories.json");
            exporter.AddExporter(DeckData.GetNationalities, "DeckNationalities.json");
            exporter.AddExporter(DeckData.GetCountries, "DeckCountries.json");
            exporter.AddExporter(DeckData.GetCoalitions, "DeckCoalitions.json");
            exporter.AddExporter(DeckData.GetUnitTypes, "DeckUnitTypes.json");
            exporter.AddExporter(DeckData.GetFactories, "UnitFactories.json");
            exporter.AddExporter(DeckData.GetDeckModifiers, "DeckModifiers.json");
            exporter.AddExporter(DeckData.GetFactionMappings, "DeckFactionMappings.json");

            exporter.AddExporter(UnitData.GetUnits, "Units.json");


            exporter.Export();
        }
        
    }
}
