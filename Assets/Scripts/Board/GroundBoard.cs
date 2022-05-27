using UnityEngine;

public class GroundBoard : Board
{
	[Header("Starting tiles")]
	[SerializeField] private Tile entrance;
	[SerializeField] private Tile foyer;
	[SerializeField] private Tile staircase;

	public override Vector2Int StartPosition => new Vector2Int(Width - 1, Height / 2);

	protected override void Awake()
    {
		if (entrance == null || foyer == null || staircase == null) 
			throw new System.NullReferenceException("Null starting tiles");

		base.Awake();
		
		var pos = StartPosition;
		tiles[pos.x, pos.y] = entrance;
		tiles[pos.x - 1, pos.y] = foyer;
		tiles[pos.x - 2, pos.y] = staircase;
    }

	public override bool TileChooser(Tile tile) => tile.Location.IsGround;

	public override char Signature => 'g';
}
