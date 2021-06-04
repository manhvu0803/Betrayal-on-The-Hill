using UnityEngine;

public class UpperBoard : Board
{
	protected override void Start()
	{
		base.Start();

		playerPosition = new Vector2Int(width / 2, height / 2);
	}

	public override void PutNewTile()
	{
		base.PutNewTile(GetUpper);
	}

	private bool GetUpper(TileData.Location location)
	{
		return location.upper;
	}
}
