using UnityEngine;

public class BasementBoard : Board
{
	[SerializeField] private Tile landing;

	protected override void Start()
	{
		if (landing == null) throw new NoStartingTileException();
		
		base.Start();

		playerPosition = new Vector2Int(width / 2, height / 2);
		tiles[playerPosition.x, playerPosition.y] = landing;
	}

	public override void PutNewTile() => base.PutNewTile(GetBasement);

	private bool GetBasement(TileData.Location location) => location.basement;
}
