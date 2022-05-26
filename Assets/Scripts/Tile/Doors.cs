using System;

[Serializable]
public struct Doors
{
	public bool north, east, south, west;

	public Doors(bool n, bool e, bool s, bool w)
	{
		north = n;
		east = e;
		south = s;
		west = w;
	}

	public Doors(Doors dir) : this(dir.north, dir.east, dir.south, dir.west) {}

	public Doors AfterRotate(int rotation)
	{
		// Push negative angles to positive
		rotation = rotation / 90 + 4;
		var dirs = new bool[4];

		dirs[rotation % 4] = north;
		dirs[(1 + rotation) % 4] = east;
		dirs[(2 + rotation) % 4] = south;
		dirs[(3 + rotation) % 4] = west;

		return new Doors(dirs[0], dirs[1], dirs[2], dirs[3]);
	}

	public override string ToString() => $"North:{north} East:{east} South:{south} West:{west}";
}
