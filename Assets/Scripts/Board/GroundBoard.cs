using UnityEngine;

public class GroundBoard : Board
{
	[Header("Starting tiles")]
	[SerializeField] private Tile entrance;
	[SerializeField] private Tile foyer;
	[SerializeField] private Tile staircase;

	protected override void Start()
    {
		if (entrance == null || foyer == null || staircase == null) throw new NoStartingTileException();

		base.Start();
		
		playerPosition = new Vector2Int(width - 1, height / 2);

		var pos = playerPosition;
		tiles[pos.x, pos.y] = entrance;
		--pos.x;
		tiles[pos.x, pos.y] = foyer;
		--pos.x;
		tiles[pos.x, pos.y] = staircase;
    }

	public override void PutNewTile() => base.PutNewTile(GetGround);

	private bool GetGround(TileData.Location location) => location.ground;
	
}
