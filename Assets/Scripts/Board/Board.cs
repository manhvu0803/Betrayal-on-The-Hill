using System;
using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
	public struct Surrounding
	{
		public enum State 
		{ 
			Door, Wall, Empty
		}

		public State north, east, south, west;

		// C# doesn't allow parameterless constructor in struct, so only this is available
		public Surrounding(State defaultState)
		{
			this.north = defaultState;
			this.east  = defaultState;
			this.south = defaultState;
			this.west  = defaultState;
		}

		public Surrounding(State north, State east, State south, State west)
		{
			this.north = north;
			this.east  = east;
			this.south = south;
			this.west  = west;
		}

		public override string ToString()
		{
			return $"n:{north} e:{east} s:{south} w:{west}";
		}
	}

	protected Tile[,] tiles;

	public virtual Vector2Int StartingPosition
	{
		get;
	}

	// A single lowercase character that represent the board 
	// 'g' for Ground, 'b' for Basement, 'u' for Upper
	public virtual char Signature
	{
		get => '0';
	}

	private Surrounding currentSurrounding;

	#region Board size
	[Header("Board size")]
	[SerializeField] protected int width;
	[SerializeField] protected int height;

	public int Width { get => width; }
	public int Height { get => height; }
	#endregion

	protected virtual void Start()
	{
		tiles = new Tile[width, height];
		StartCoroutine(LateStart());
    }

	IEnumerator LateStart()
	{
		yield return new WaitForEndOfFrame();
	}

	public void Reset()
	{
		for (int i = 0; i < width; ++i) 
			for (int j = 0; j < height; ++j)
				if (tiles[i, j]?.IsStartingTile() ?? false) tiles[i, j] = null;
	}

	public Surrounding GetSurrounding(Vector2Int pos)
	{
		int x = pos.x, y = pos.y;
		var srd = new Surrounding(Surrounding.State.Wall);

		// Check north side
		if (y >= height - 1 || tiles[x, y + 1] == null) srd.north = Surrounding.State.Empty;
		else if (tiles[x, y + 1].GetDoors().south) srd.north = Surrounding.State.Door;
		// Check east side
		if (x >= width - 1 || tiles[x + 1, y] == null) srd.east = Surrounding.State.Empty;
		else if (tiles[x + 1, y].GetDoors().west) srd.east = Surrounding.State.Door;
		// Check south side
		if (y <= 0 || tiles[x, y - 1] == null) srd.south = Surrounding.State.Empty;
		else if (tiles[x, y - 1].GetDoors().north) srd.south = Surrounding.State.Door;
		// Check west side
		if (x <= 0 || tiles[x - 1, y] == null) srd.west = Surrounding.State.Empty;
		else if (tiles[x - 1, y].GetDoors().east) srd.west = Surrounding.State.Door;

		return srd;
	}

	public void Rotate(int direction) => tiles[0, 0].transform.Rotate(0, 0, NextValidRotation(tiles[0, 0], this.currentSurrounding, direction));

	public virtual bool TileChooser(Tile.Location location) => true;

	public Tile TileAt(int x, int y)
	{
		return tiles[x, y];
	}

	public Tile TileAt(Vector2Int pos)
	{
		return TileAt(pos.x, pos.y);
	}

	public void PutNewTile(Vector2Int pos, Tile newTile)
	{
		newTile.transform.parent = this.transform;
		this.tiles[pos.x, pos.y] = newTile;
	}

	public Vector3 BoardPositionToWorld(Vector2Int pos, float y)
	{
		var trf = this.transform;
		float x = pos.x * trf.lossyScale.x + trf.position.x;
		float z = pos.y * trf.lossyScale.z + trf.position.z;
		return new Vector3(x, y ,z);
	}

	public Vector3 BoardPositionToWorld(Vector2Int pos) => BoardPositionToWorld(pos, this.transform.position.y);

	static public float NextValidRotation(Tile tile, Surrounding srd, int direction)
	{
		int maxDoor = -1;
		int angle = 0;
		var doors = tile.GetDoors();
		
		if (direction >= 0) direction = 1;
		else direction = -1;
		
		for (int i = 0; i < 4; ++i) {
			Debug.Log(doors);
			int cnt = 0;
			if ((doors.north && srd.north == Surrounding.State.Door) || (!doors.north && srd.north == Surrounding.State.Wall)) ++cnt;
			if ((doors.east  && srd.east  == Surrounding.State.Door) || (!doors.east  && srd.east  == Surrounding.State.Wall)) ++cnt;
			if ((doors.south && srd.south == Surrounding.State.Door) || (!doors.south && srd.south == Surrounding.State.Wall)) ++cnt;
			if ((doors.west  && srd.west  == Surrounding.State.Door) || (!doors.west  && srd.west  == Surrounding.State.Wall)) ++cnt;

			// Including equal doors count to find the next	valid rotation
			if (maxDoor <= cnt) {
				maxDoor = cnt;
				// This rotate direction is the opposite of transform.eulerAngles.y, which hold the real rotation
				angle = -i * direction * 90; 
			}

			doors = doors.AfterRotate(direction * 90);
		}

		return angle;
	}
}
