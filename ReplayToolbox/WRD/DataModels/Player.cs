using Newtonsoft.Json;
using ReplayToolbox.WRD.Json;

namespace ReplayToolbox.WRD.DataModels
{
    public class Player
    {
        public string PlayerUserId { get; set; }
        public int PlayerRank { get; set; }
        [JsonConverter(typeof(StringToBooleanJsonConverter))]
        public bool PlayerObserver { get; set; }
        public int PlayerAlliance { get; set; }
        [JsonConverter(typeof(StringToBooleanJsonConverter))]
        public bool PlayerReady { get; set; }
        public double PlayerElo { get; set; }
        public int PlayerLevel { get; set; }
        public string PlayerName { get; set; }
        public string PlayerTeamName { get; set; }
        public string PlayerAvatar { get; set; }
        public string PlayerDeckName { get; set; }
        public string PlayerDeckContent { get; set; }
        [JsonConverter(typeof(StringToBooleanJsonConverter))]
        public bool PlayerIsEnteredInLobby { get; set; }
        public int PlayerScoreLimit { get; set; }
        public int PlayerIncomeRate { get; set; }
    }
}
