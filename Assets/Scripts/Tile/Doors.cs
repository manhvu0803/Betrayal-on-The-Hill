using System;
using System.Text;

[Serializable, Flags]
public enum DoorDirections
{
	None = 0,
	North = 1,
	East = 2,
	South = 4,
	West = 8
}

public static class DoorDirectionsExtension
{
	private static StringBuilder StrBuilder = new();

	private static bool[] Temp = new bool[4];

	public static DoorDirections DoorDirectionsFrom(bool north, bool east, bool south, bool west)
	{
		DoorDirections directions = DoorDirections.None;

		if (north)
		{
			directions |= DoorDirections.North;
		}
		if (east)
		{
			directions |= DoorDirections.East;
		}	
		if (south)
		{
			directions |= DoorDirections.South;
		}	
		if (west)
		{
			directions |= DoorDirections.West;
		}

		return directions;
	}

	public static DoorDirections AfterRotate(this DoorDirections doors, int rotation)
	{
		// Push negative angles to positive
		rotation = rotation / 90 + 4;

		Temp[rotation % 4] = doors.OpenTo(DoorDirections.North);
		Temp[(1 + rotation) % 4] = doors.OpenTo(DoorDirections.East);
		Temp[(2 + rotation) % 4] = doors.OpenTo(DoorDirections.South);
		Temp[(3 + rotation) % 4] = doors.OpenTo(DoorDirections.West);

		return DoorDirectionsFrom(Temp[0], Temp[1], Temp[2], Temp[3]);
	}

	public static bool OpenNorth(this DoorDirections directions) => directions.OpenTo(DoorDirections.North);

	public static bool OpenEast(this DoorDirections directions)  => directions.OpenTo(DoorDirections.East);

	public static bool OpenSouth(this DoorDirections directions) => directions.OpenTo(DoorDirections.South);

	public static bool OpenWest(this DoorDirections directions)  => directions.OpenTo(DoorDirections.West);

	public static bool OpenTo(this DoorDirections directions, DoorDirections containedDirections)
	{
		return directions.HasFlag(containedDirections);
	}

	public static string ToString(this DoorDirections directions)
	{
		StrBuilder.Clear();
		
		StrBuilder.Append("North:");
		StrBuilder.Append(directions.OpenTo(DoorDirections.North));
		StrBuilder.Append(" East:");
		StrBuilder.Append(directions.OpenTo(DoorDirections.East));
		StrBuilder.Append(" South:");
		StrBuilder.Append(directions.OpenTo(DoorDirections.South));
		StrBuilder.Append(" West:");
		StrBuilder.Append(directions.OpenTo(DoorDirections.West));

		return StrBuilder.ToString();
	}
}
