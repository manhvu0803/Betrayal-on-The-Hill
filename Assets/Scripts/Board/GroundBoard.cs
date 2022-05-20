using UnityEngine;

public class GroundBoard : Board
{
	[Header("Starting tiles")]
	[SerializeField] private Tile entrance;
	[SerializeField] private Tile foyer;
	[SerializeField] private Tile staircase;

	public override Vector2Int StartPosition => new Vector2Int(_width - 1, _height / 2);

	protected override void Awake()
    {
		if (entrance == null || foyer == null || staircase == null) 
			throw new System.NullReferenceException("Null starting tiles");

		base.Awake();
		
		var pos = StartPosition;
		tiles[pos.x, pos.y] = entrance;
		--pos.x;
		tiles[pos.x, pos.y] = foyer;
		--pos.x;
		tiles[pos.x, pos.y] = staircase;
    }

	public override bool TileChooser(Tile.Location location) => location.ground;

	public override char Signature => 'g';
}
