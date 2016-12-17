using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ReplayToolbox.Tests
{
    public class ReplayTest
    {
        [Theory]
        [InlineData(@"ReplayToolbox.Tests\TestData\testreplay.wargamerpl2")]
        public async void CreateReplayFromFile(string replayFile)
        {
            var replay = await ReplayToolbox.WRD.Replay.FromFile(replayFile);
            Assert.True(replay.ReplayHeader.game.InitMoney == 3000);
            Assert.Equal("Conquete_3x3_Highway_Small", replay.ReplayHeader.game.Map);
            Assert.True(replay.ReplayHeader.Players.Count == 6);
        }
    }
}
