namespace ReplayToolbox.WRD.DataModels
{
    public enum NationConstraint : long
    {
        NoConstraint = 4294967295,
        NationOrCoalition = 0,
        Nation = 1
    }

    public enum ThematicConstraint : long
    {
        NoConstraint = 4294967295,
        Motorized = 0,
        Armored = 1,        
        Support = 2,
        Marines = 3,
        Mechanized = 4,
        Airborne = 5,
        Navy = 6
    }

    public enum DateConstraint : long
    {
        NoConstraint = 4294967295,
        Before1985 = 0,
        Before1980 = 1
    }
}
