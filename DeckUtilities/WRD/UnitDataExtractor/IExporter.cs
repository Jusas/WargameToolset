using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitExtractor.DataModels;

namespace UnitExtractor
{
    public interface IExporter
    {
        void AddExporter(Func<Configuration, DataSource, object> extractorMethod, string outputFilename);
        void Export();
    }
}
