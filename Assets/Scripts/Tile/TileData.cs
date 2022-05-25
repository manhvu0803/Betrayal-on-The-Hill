using UnityEngine;

public class TileData : ScriptableObject
{
	[field: SerializeField] 
	public string TileName { get; private set; }
	
	[field: SerializeField]
	public bool IsStartingTile { get; private set; }

	[field: SerializeField] 
	public Doors Doors { get; private set; }

	[field: SerializeField] 
	public Texture Texture { get; private set; }

	[field: SerializeField]
	public TileLocation Location { get; private set; }
}