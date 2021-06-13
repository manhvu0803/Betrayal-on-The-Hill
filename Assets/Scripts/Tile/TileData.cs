using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tile/Map tile")]
public class TileData : ScriptableObject
{
	[Serializable]
	public struct Location
	{
		public bool upper, ground, basement;

		public Location(Location loc)
		{
			upper = loc.upper;
			ground = loc.ground;
			basement = loc.basement;
		}

		public override string ToString() => $"u:{upper} g:{ground} b:{basement}";
	}

	[SerializeField] public string tileName;
	
	[SerializeField] public bool isStartingTile;

	[SerializeField] public Location location;

	[SerializeField] public Direction doors;

	[SerializeField] public Texture texture;
}
