using System;
using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
	public class NoStartingTileException : System.Exception
	{
		public NoStartingTileException() { }
		public NoStartingTileException(string message) : base(message) { }
		public NoStartingTileException(string message, System.Exception inner) : base(message, inner) { }
		protected NoStartingTileException(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}

	protected TilePool tilePool;

	protected TileData startTileData;

	protected Tile[,] tiles;

	[ReadOnlyField] [SerializeField] protected Vector2Int playerPosition;

	private int[] currentSurrounding;

	[Header("Board size")]
	[SerializeField] protected int width;
	[SerializeField] protected int height;

	protected virtual void Start()
	{
		tiles = new Tile[width, height];
		StartCoroutine(LateStart());
    }

	IEnumerator LateStart()
	{
		yield return new WaitForEndOfFrame();
		tilePool = GetComponentInParent<BoardMaster>().tilePool;
	}

	public Vector2Int CurrentPosition()
	{
		return playerPosition;
	}

	public Tile CurrentTile()
	{
		return tiles[playerPosition.x, playerPosition.y];
	}

	public void Reset()
	{
		for (int i = 0; i < width; ++i) 
			for (int j = 0; j < height; ++j)
				if (tiles[i, j] != null && !tiles[i, j].IsStartingTile()) tiles[i, j] = null;
	}

	protected int[] Surrounding()
	{
		int x = playerPosition.x, y = playerPosition.y;
		var surrounding = new int[4] {-1, -1, -1, -1};

		// Check north side
		if (y >= height - 1 || tiles[x, y + 1] == null) surrounding[0] = 0;
		else if (tiles[x, y + 1].GetDoors().south) surrounding[0] = 1;
		// Check east side
		if (x >= width - 1 || tiles[x + 1, y] == null) surrounding[1] = 0;
		else if (tiles[x + 1, y].GetDoors().west) surrounding[1] = 1;
		// Check south side
		if (y <= 0 || tiles[x, y - 1] == null) surrounding[2] = 0;
		else if (tiles[x, y - 1].GetDoors().north) surrounding[2] = 1;
		// Check west side
		if (x <= 0 || tiles[x - 1, y] == null) surrounding[3] = 0;
		else if (tiles[x - 1, y].GetDoors().east) surrounding[3] = 1;

		return surrounding;
	}

	public void MovePlayer(Vector2Int movement)
	{
		var pos = this.playerPosition;

		if (tiles[pos.x, pos.y] != null) tiles[pos.x, pos.y].OnExit();
		pos += movement;
		pos.Clamp(new Vector2Int(0, 0), new Vector2Int(width - 1, height - 1));
		if (tiles[pos.x, pos.y] != null) tiles[pos.x, pos.y].OnEnter();

		this.playerPosition = pos;
	}

	public void Rotate(int direction)
	{
		CurrentTile().transform.Rotate(0, 0, NextValidAngle(CurrentTile(), this.currentSurrounding, direction));
	}

	public virtual void PutNewTile()
	{
		var surrounding = Surrounding();
		if (surrounding[0] != 1 && surrounding[1] != 1 && surrounding[2] != 1 && surrounding[3] != 1) return;
		
		var tile = tilePool.GetTile();
		if (tile == null) return;

		MoveTile(tile, surrounding);
	}
	
	protected void PutNewTile(Func<TileData.Location, bool> getter)
	{
		var surrounding = Surrounding();
		if (surrounding[0] != 1 && surrounding[1] != 1 && surrounding[2] != 1 && surrounding[3] != 1) return;

		var tile = tilePool.GetTile(getter);
		if (tile == null) return;

		MoveTile(tile, surrounding);
	}

	private void MoveTile(Tile tile, int[] surrounding)
	{
		tile.transform.position = CurrentPositionToWorld();
		tile.transform.Rotate(0, 0, NextValidAngle(tile, surrounding, 1));
		tile.transform.parent = this.transform;
		tiles[playerPosition.x, playerPosition.y] = tile;
		tile.OnDiscover();
		this.currentSurrounding = surrounding;
	}

	public Vector3 CurrentPositionToWorld(float y)
	{
		var trf = this.transform;
		float x = playerPosition.x * trf.lossyScale.x + trf.position.x;
		float z = playerPosition.y * trf.lossyScale.z + trf.position.z;
		return new Vector3(x, y ,z);
	}

	public Vector3 CurrentPositionToWorld()
	{
		return CurrentPositionToWorld(this.transform.position.y);
	}

	private float NextValidAngle(Tile tile, int[] surrounding, int direction)
	{
		int maxDoor = -1;
		int angle = 0;
		var doors = tile.GetDoors();
		
		for (int i = 0; i < 4; ++i) {
			int cnt = 0;
			if ((doors.north && surrounding[0] == 1) || (!doors.north && surrounding[0] == -1)) ++cnt;
			if ((doors.east  && surrounding[1] == 1) || (!doors.east  && surrounding[1] == -1)) ++cnt;
			if ((doors.south && surrounding[2] == 1) || (!doors.south && surrounding[2] == -1)) ++cnt;
			if ((doors.west  && surrounding[3] == 1) || (!doors.west  && surrounding[3] == -1)) ++cnt;

			// Including equal doors count to find the next rotation
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
