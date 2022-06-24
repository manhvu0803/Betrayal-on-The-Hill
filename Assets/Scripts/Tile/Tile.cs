using UnityEngine;

public class Tile : MonoBehaviour
{
	[field: SerializeField] 
	public string TileName { get; private set; }
	
	[field: SerializeField] 
	public Texture Texture { get; private set; }

	[field: SerializeField]
	public bool IsStartingTile { get; private set; }

	public MonoBehaviour script;

	[SerializeField] 
	private DoorDirections _doors;

	[field: SerializeField]
	public TileLocation Location { get; private set; }

	public DoorDirections Doors => _doors.AfterRotate((int)transform.rotation.eulerAngles.y);

	public void Initialize(GameObject meshPrefab, Vector2Int pos)
	{
		var newMesh = Instantiate(meshPrefab);

		newMesh.transform.parent = this.transform;
		newMesh.transform.localPosition = Vector3.zero;
		newMesh.transform.localRotation = Quaternion.Euler(0, 0, 0);

		newMesh.GetComponent<MeshRenderer>().material.mainTexture = Texture;
		
		this.name = $"Tile_{pos.x}_{pos.y}_{TileName}";
		this.transform.position = new Vector3(pos.x, 0, pos.y);
	}
	
	public override string ToString() => name;
	
	public virtual void OnDiscover() {}
	
	public virtual void OnEnter() {}
	
	public virtual void OnActivate() {}

	public virtual void OnExit() {}

	public virtual void OnEndTurn() {}
}
