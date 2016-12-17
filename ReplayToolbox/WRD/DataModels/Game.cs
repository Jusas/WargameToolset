using Newtonsoft.Json;
using ReplayToolbox.WRD.Json;

namespace ReplayToolbox.WRD.DataModels
{
    public class Game
    {
        public int GameMode { get; set; }
        [JsonConverter(typeof(StringToBooleanJsonConverter))]
        public bool IsNetworkMode { get; set; }
        public int NbMaxPlayer { get; set; }
        public int NbPlayer { get; set; }
        public string Seed { get; set; }
        [JsonConverter(typeof(StringToBooleanJsonConverter))]
        public bool Private { get; set; }
        public string ServerName { get; set; }
        [JsonConverter(typeof(StringToBooleanJsonConverter))]
        public bool WithHost { get; set; }
        public string ServerProtocol { get; set; }
        public int TimeLeft { get; set; }
        public string Version { get; set; }
        public object GameState { get; set; }
        [JsonConverter(typeof(StringToBooleanJsonConverter))]
        public bool NeedPassword { get; set; }
        public object GameType { get; set; }
        public string Map { get; set; }
        public int InitMoney { get; set; }
        public int TimeLimit { get; set; }
        public int ScoreLimit { get; set; }
        public int NbIA { get; set; }
        public int NbAI { get; set; }
        public string VictoryCond { get; set; }
        public int IncomeRate { get; set; }
        public int WarmupCountdown { get; set; }
        public int DeploiementTimeMax { get; set; }
        public int DebriefingTimeMax { get; set; }
        public int LoadingTimeMax { get; set; }
        public int NbMinPlayer { get; set; }
        public int DeltaMaxTeamSize { get; set; }
        public int MaxTeamSize { get; set; }
        public NationConstraint NationConstraint { get; set; }
        public ThematicConstraint ThematicConstraint { get; set; }
        public DateConstraint DateConstraint { get; set; }
    }
}
