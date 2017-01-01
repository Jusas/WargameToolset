using System;
using System.Collections.Generic;
using System.Text;
using DeckToolbox.WRD.DataModels;

namespace DeckToolbox.WRD.Resolvers
{
    public interface IUnitResolver
    {
        Unit GetUnitData(int unitId);
        Unit GetUnitData(int factionId, int unitDeckId);
    }
}
