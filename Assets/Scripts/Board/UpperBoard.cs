using UnityEngine;

public class UpperBoard : Board
{
	[SerializeField] private Tile landing;

	public override Vector2Int StartPosition => new Vector2Int(Width / 2, Height / 2);

	protected override void Awake()
	{
		if (landing == null) throw new System.NullReferenceException("Null upper landing");
		
		base.Awake();

		var pos = StartPosition;
		tiles[pos.x, pos.y] = landing;
	}

	public override bool TileChooser(Tile tile) => tile.Location.HasFlag(TileLocation.Upper);
}
