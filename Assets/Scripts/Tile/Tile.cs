using System;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
	[SerializeField] private TileData data;

	private void Start()
	{
		if (!data.isStartingTile)
			GetComponentInChildren<MeshRenderer>().material.mainTexture = data.texture;
	}

	public virtual void OnDiscover() {}
	public virtual void OnEnter() {}
	public virtual void OnExit() {}

	public Direction GetDoors()
	{
		// Don't look at the rotation in the inspector. It's a lie :(
		return new Direction(data.doors.AfterRotate((int)transform.rotation.eulerAngles.y));
	}

	public TileData.Location GetLocation()
	{
		return data.location;
	}

	public bool IsStartingTile()
	{
		return data.isStartingTile;
	}

	public void SetData(Vector2Int pos, TileData tileData)
	{
		name = $"Tile_{pos.x}_{pos.y}";
		transform.localPosition = new Vector3(pos.x, 0, pos.y);
	}

	public override string ToString()
	{
		return $"{name}__{data.tileName}";
	}
}
