using UnityEngine;

public class GroundBoard : Board
{
	[Header("Starting tiles")]
	[SerializeField] private Tile entrance;
	[SerializeField] private Tile foyer;
	[SerializeField] private Tile staircase;

	public override Vector2Int StartingPosition => new Vector2Int(width - 1, height / 2);

	protected override void Start()
    {
		if (entrance == null || foyer == null || staircase == null) 
			throw new System.NullReferenceException("Null starting tiles");

		base.Start();
		
		var pos = StartingPosition;
		tiles[pos.x, pos.y] = entrance;
		--pos.x;
		tiles[pos.x, pos.y] = foyer;
		--pos.x;
		tiles[pos.x, pos.y] = staircase;
    }

	public override bool TileChooser(Tile.Location location) => location.ground;

	public override char Signature => 'g';
}
