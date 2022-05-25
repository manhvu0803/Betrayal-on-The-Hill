using System;
using UnityEngine;

public abstract class Board : MonoBehaviour
{
	protected Tile[,] tiles;

	public virtual Vector2Int StartPosition
	{
		get;
	}

	// A single lowercase character that represent the board
	// 'g' for Ground, 'b' for Basement, 'u' for Upper
	public abstract char Signature { get; }

	#region Board size
	[field: SerializeField]
	public int Width { get; protected set; }

	[field: SerializeField]
	public int Height { get; protected set; }

	#endregion

	protected virtual void Awake()
	{
		tiles = new Tile[Width, Height];
    }

	public void Reset()
	{
		for (int i = 0; i < Width; ++i)
			for (int j = 0; j < Height; ++j)
				if (tiles[i, j]?.IsStartingTile ?? false) tiles[i, j] = null;
	}

	public virtual bool TileChooser(TileLocation location) => true;

	public Tile TileAt(int x, int y) => tiles[x, y];

	public Tile TileAt(Vector2Int pos) => TileAt(pos.x, pos.y);

	public void PutNewTile(Vector2Int pos, Tile newTile)
	{
		newTile.transform.parent = this.transform;
		this.tiles[pos.x, pos.y] = newTile;
	}

	public Vector3 BoardPositionToWorld(Vector2Int pos, float y)
	{
		Transform transf = this.transform;
		float x = pos.x + transf.position.x - Width / 2;
		float z = pos.y + transf.position.z - Height / 2;
		return new Vector3(x, y, z);
	}

	public Vector3 BoardPositionToWorld(Vector2Int pos) => BoardPositionToWorld(pos, this.transform.position.y);

	public Vector2Int GetSquareFromWorldPoint(Vector3 worldPoint)
	{
		Vector3 localPoint = this.transform.InverseTransformPoint(worldPoint);
		return GetSquareFromLocalPoint(localPoint);
	}

	private Vector2Int GetSquareFromLocalPoint(Vector3 point)
	{
		int x = (int)Math.Round(point.x) + Width / 2;
		int y = (int)Math.Round(point.z) + Height / 2;
		return new Vector2Int(x, y);
	}
}
