using UnityEngine;

public class GroundBoard : Board
{
	[Header("Starting tiles")]
	[SerializeField] private Tile entrance;
	[SerializeField] private Tile foyer;
	[SerializeField] private Tile staircase;

	protected override void Start()
    {
		base.Start();
		
		playerPosition = new Vector2Int(width - 1, height / 2);

		var pos = playerPosition;
		SetStartTile(pos, entrance);
		--pos.x;
		SetStartTile(pos, foyer);
		--pos.x;
		SetStartTile(pos, staircase);
    }

	public override void PutNewTile()
	{
		base.PutNewTile(GetGround);
	}

	private bool GetGround(TileData.Location location)
	{
		return location.ground;
	}
	
}
