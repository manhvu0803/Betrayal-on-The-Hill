using UnityEngine;

public class UpperBoard : Board
{
	[SerializeField] private Tile landing;

	public override Vector2Int StartingPosition => new Vector2Int(_width / 2, _height / 2);

	protected override void Awake()
	{
		if (landing == null) throw new System.NullReferenceException("Null upper landing");
		
		base.Awake();

		var pos = StartingPosition;
		tiles[pos.x, pos.y] = landing;
	}

	public override bool TileChooser(Tile.Location location) => location.upper;

	public override char Signature => 'u';
}
