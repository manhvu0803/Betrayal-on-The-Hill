using System;

[Serializable]
public struct TileLocation
{
    public bool IsUpper;
    public bool IsGround;
    public bool IsBasement;

    public override string ToString() 
    {
        string str = "";
        str += (IsUpper)? "u" : "";
        str += (IsGround)? "g" : "";
        str += (IsBasement)? "b" : "";
        return str;
    }
}