using UnityEngine;

public class BasementBoard : Board
{
	[SerializeField] private Tile landing;

	public override Vector2Int StartingPosition => new Vector2Int(width / 2, height / 2);

	protected override void Start()
	{
		if (landing == null) throw new System.NullReferenceException("Null basement landing");
		
		base.Start();

		var pos = StartingPosition;
		tiles[pos.x, pos.y] = landing;
	}

	public override bool TileChooser(Tile.Location location) => location.basement;
	
	public override char Signature => 'b';
}
