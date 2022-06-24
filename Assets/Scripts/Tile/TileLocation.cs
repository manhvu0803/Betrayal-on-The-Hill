using System;

[Serializable, Flags]
public enum TileLocation
{
    Upper = 1,
    Ground = 2,
    Basement = 4
}