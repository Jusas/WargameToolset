using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeckToolbox.WRD
{
    /// <summary>
    /// These were dug from replay files, knowing that the first 9 bits
    /// define the nation/coalition (the first bit defines faction, 0 = blue, 1 = red).
    /// Couldn't find the corresponding values from the game data files.
    /// Would have expected to find it from TShowRoomDeckSerializer but there
    /// aren't any IDs stored, only a list of TableStrings.
    /// </summary>
    public enum NationCoalitionx : uint
    {
        // Bluefor nations
        US = 10,
        UK = 26,
        FR = 42,
        RFA = 58,
        CAN = 74,
        DAN = 90,
        SWE = 106,
        NOR = 122,
        ROK = 170,
        JAP = 154,
        ANZ = 138,
        HOL = 186,
        // Bluefor coalitions
        /*
        EURO = 176,
        SCAND = 177,
        CMW = 178,
        BLUEDRAG = 179,
        LAND = 182,
        NORAD = 184,
        NLGR = 201,
        */
        EURO = 192,
        SCAND = 193,
        CMW = 194,
        BLUEDRAG = 195,
        LAND = 198,
        NORAD = 200,
        NLGR = 201,
        // Redfor nations
        RDA = 266,
        URSS = 282,
        POL = 298,
        TCH = 314,
        CHI = 330,
        NK = 346,
        // Redfor coalitions
        REDDRAG = 356,
        NSWP = 357,
        JUCHE = 359,
        // Mixed redfor/bluefor (no nation or coalition)
        BLUEFOR = 185,
        REDFOR = 361
    }

    // Manually dug from the TShowRoomDeckSerializer, these
    // correspond to the index values of the Categories list.
    // None == 1985+
    public enum Eraz : uint
    {   
        BeforeAnd1980 = 0,
        BeforeAnd1985 = 1,
        None = 2
    }

    // Manually dug from the TShowRoomDeckSerializer, these
    // correspond to the index values if the UnitTypes list,
    // with the addition of "none" which is not listed in the
    // UnitTypes list.
    public enum Specializationz : uint
    {
        Motorized = 0,
        Armored = 1,
        Support = 2,
        Marine = 3,
        Mechanized = 4,
        Airborne = 5,
        Navy = 6,
        None = 7
    }

    public enum Factionz : uint
    {
        BLUEFOR = 0,
        REDFOR = 1
    }
}
