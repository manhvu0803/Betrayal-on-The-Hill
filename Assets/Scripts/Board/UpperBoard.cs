using UnityEngine;

public class UpperBoard : Board
{
	[SerializeField] private Tile landing;

	public override Vector2Int StartingPosition => new Vector2Int(width / 2, height / 2);

	protected override void Start()
	{
		if (landing == null) throw new System.NullReferenceException("Null upper landing");
		
		base.Start();

		var pos = StartingPosition;
		tiles[pos.x, pos.y] = landing;
	}

	public override bool TileChooser(Tile.Location location) => location.upper;

	public override char Signature => 'u';
}
