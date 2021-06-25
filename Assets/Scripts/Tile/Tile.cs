using System;
using UnityEngine;

public class Tile : MonoBehaviour
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

	[SerializeField] private string tileName;
	
	[SerializeField] private bool isStartingTile;

	[SerializeField] private Location location;

	[SerializeField] private Direction doors;

	[SerializeField] private Texture texture;

	public virtual void OnDiscover() {}
	public virtual void OnEnter() {}
	public virtual void OnExit() {}

	// The rotation in the inspector is a lie :(
	public Direction GetDoors() => new Direction(doors.AfterRotate((int)transform.rotation.eulerAngles.y));

	public Location GetLocation() => location;

	public bool IsStartingTile() => isStartingTile;

	public void Initialize(GameObject meshPrefab)
	{
		var newMesh = Instantiate(meshPrefab);
		newMesh.transform.parent = this.transform;
		
		// Set the mesh transform to properly follow its tile object
		newMesh.transform.localPosition = Vector3.zero;
		newMesh.transform.localRotation = Quaternion.Euler(0, 0, 0);

		newMesh.GetComponent<MeshRenderer>().material.mainTexture = texture;	
	}

	public void Initialize(GameObject meshPrefab, Vector2Int pos, float rotation)
	{
		Initialize(meshPrefab);
		this.name = $"Tile-{pos.x}_{pos.y}_{tileName}";
		this.transform.localPosition = new Vector3(pos.x, 0, pos.y);
		this.transform.Rotate(0, 0, rotation);
	}

	public override string ToString() => name;
}
