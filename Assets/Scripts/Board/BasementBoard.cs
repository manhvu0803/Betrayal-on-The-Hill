using UnityEngine;

public class BasementBoard : Board
{
	[SerializeField] private Tile landing;

	public override Vector2Int StartPosition => new Vector2Int(_width / 2, _height / 2);

	protected override void Awake()
	{
		if (landing == null) throw new System.NullReferenceException("Null basement landing");
		
		base.Awake();

		var pos = StartPosition;
		tiles[pos.x, pos.y] = landing;
	}

	public override bool TileChooser(Tile.Location location) => location.basement;
	
	public override char Signature => 'b';
}
