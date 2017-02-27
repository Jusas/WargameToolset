using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitExtractor.DataModels
{
    public class ExportedDataTable
    {
        public class MetaDataModel
        {
            public string DataIdentifier { get; set; }
            public string[] ExtractedFrom { get; set; }
            public string ExtractionDate { get; set; }
        }
        
        public MetaDataModel MetaData { get; set; }
        public object Data { get; set; }
    }
}
