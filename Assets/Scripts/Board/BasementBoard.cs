using UnityEngine;

public class BasementBoard : Board
{
	protected override void Start()
	{
		base.Start();

		playerPosition = new Vector2Int(width / 2, height / 2);
	}

	public override void PutNewTile()
	{
		base.PutNewTile(GetBasement);
	}

	private bool GetBasement(TileData.Location location)
	{
		return location.basement;
	}
}
