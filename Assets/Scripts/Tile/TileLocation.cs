using System;

[Serializable]
public struct TileLocation
{
    public bool upper, ground, basement;

    public override string ToString() 
    {
        string str = "";
        str += (upper)? "u" : "";
        str += (ground)? "g" : "";
        str += (basement)? "b" : "";
        return str;
    }
}