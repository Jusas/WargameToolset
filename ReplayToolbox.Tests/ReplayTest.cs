using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ReplayToolbox.WRD;
using Xunit;

namespace ReplayToolbox.Tests
{
    public class ReplayTest
    {
        [Theory]
        [InlineData(@"testreplay.wargamerpl2")]
        public async void CreateReplayFromFile(string replayFile)
        {
            var asm = GetType().GetTypeInfo().Assembly;
            var resource = asm.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(replayFile));
            Replay replay;
            using (var replayStream = asm.GetManifestResourceStream(resource))
            {
                replay = await Replay.FromStream(replayStream);
            }
            
            Assert.True(replay.ReplayHeader.game.InitMoney == 3000);
            Assert.Equal("Conquete_3x3_Highway_Small", replay.ReplayHeader.game.Map);
            Assert.True(replay.ReplayHeader.Players.Count == 6);
        }
    }
}
