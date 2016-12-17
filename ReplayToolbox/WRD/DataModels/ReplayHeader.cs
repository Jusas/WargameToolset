using System.Collections.Generic;
using Newtonsoft.Json;
using ReplayToolbox.WRD.Json;

namespace ReplayToolbox.WRD.DataModels
{
    [JsonConverter(typeof(ReplayHeaderConverter))]
    public class ReplayHeader
    {
        public Game game { get; set; }
        // needs a Custom jsonconverter
        public List<Player> Players { get; set; }

        public ReplayHeader()
        {
            Players = new List<Player>();
        }
    }
}
