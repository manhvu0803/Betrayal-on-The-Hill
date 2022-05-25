using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField]
	private TileData data;

	public bool IsStartingTile 
	{
		get => data.IsStartingTile;
	}

	public TileLocation Location 
	{
		get => data.Location;
	}

	public Doors GetDoors() => data.Doors.AfterRotate((int)transform.rotation.eulerAngles.y);


	public virtual void OnDiscover() {}
	public virtual void OnEnter() {}
	public virtual void OnExit() {}
	
	public void Initialize(GameObject meshPrefab, Vector2Int pos, float rotation)
	{
		var newMesh = Instantiate(meshPrefab);
		
		// Set the mesh transform to properly follow its tile object
		newMesh.transform.parent = this.transform;
		newMesh.transform.localPosition = Vector3.zero;
		newMesh.transform.localRotation = Quaternion.Euler(0, 0, 0);

		newMesh.GetComponent<MeshRenderer>().material.mainTexture = data.Texture;
		
		this.name = $"Tile_{pos.x}_{pos.y}_{data.TileName}";
		this.transform.position = new Vector3(pos.x, 0, pos.y);
		this.transform.Rotate(0, 0, rotation);
	}

	public override string ToString() => name;
}
