using System;
using UnityEngine;

public class Tile : MonoBehaviour
{

	[Serializable]
	public struct Location
	{
		public bool upper, ground, basement;

		public override string ToString() 
		{
			string str = "";
			str += (upper)? "u" : "";
			str += (ground)? "g" : "";
			str += (basement)? "b" : "";
			return str;
		}
	}

	[SerializeField] private string tileName;
	
	[SerializeField] private bool isStartingTile;

	[SerializeField] private Location location;

	[SerializeField] private Doors doors;

	[SerializeField] private Texture texture;

	public virtual void OnDiscover() {}
	public virtual void OnEnter() {}
	public virtual void OnExit() {}

	// The rotation in the inspector is a lie :(
	public Doors GetDoors() => new Doors(doors.AfterRotate((int)transform.rotation.eulerAngles.y));

	public Location GetLocation() => location;

	public bool IsStartingTile() => isStartingTile;

	public void Initialize(GameObject meshPrefab, Vector2Int pos, float rotation)
	{
		var newMesh = Instantiate(meshPrefab);
		
		// Set the mesh transform to properly follow its tile object
		newMesh.transform.parent = this.transform;
		newMesh.transform.localPosition = Vector3.zero;
		newMesh.transform.localRotation = Quaternion.Euler(0, 0, 0);

		newMesh.GetComponent<MeshRenderer>().material.mainTexture = texture;
		
		this.name = $"Tile_{pos.x}_{pos.y}_{tileName}";
		this.transform.position = new Vector3(pos.x, 0, pos.y);
		this.transform.Rotate(0, 0, rotation);
	}

	public override string ToString() => name;
}
