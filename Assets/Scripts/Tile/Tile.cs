using UnityEngine;

public class Tile : MonoBehaviour
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

	// The rotation in the inspector is a lie :(
	public Direction GetDoors() => new Direction(data.doors.AfterRotate((int)transform.rotation.eulerAngles.y));

	public TileData.Location GetLocation() => data.location;

	public bool IsStartingTile() => data.isStartingTile;

	public void SetData(Vector2Int pos, TileData tileData)
	{
		name = $"Tile_{pos.x}_{pos.y}";
		transform.localPosition = new Vector3(pos.x, 0, pos.y);
	}

	public override string ToString() => $"{name}__{data.tileName}";
}
