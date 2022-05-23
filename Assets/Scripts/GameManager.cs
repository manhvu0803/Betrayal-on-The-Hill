using Mirror;
using UnityEngine;
using System.Collections;

public class GameManager : NetworkBehaviour
{
	[SerializeField] private GameObject tileMeshPrefab;

	[SerializeField] private TilePool tilePool;

	[Header("Game boards")]
	[SerializeField] private Board groundBoard;
	[SerializeField] private Board upperBoard;
	[SerializeField] private Board basementBoard;

	[ReadOnlyField] [SerializeField] private NetworkPlayer[] players;

	private Surrounding _currentSurrounding = null;

	public override void OnStartServer()
	{
		base.OnStartServer();
		if (tilePool == null) throw new System.NullReferenceException("tilePool is null");
		//StartCoroutine(GetPlayers(GameObject.FindObjectOfType<NetworkRoomManager>().numPlayers));
	}

	[Server]
	IEnumerator GetPlayers(int playerCount)
	{
		// Polling every 2 frames until found all player object
		// Since the build keep failing to find remote player
		do 
		{
			yield return null;
			yield return null;
			players = GameObject.FindObjectsOfType<NetworkPlayer>();
		}
		while (players.Length < playerCount);

		foreach (var player in players) 
		{
			player.currentBoard = groundBoard;
			player.position = groundBoard.StartPosition;
		}
	}
	
	//[Server]
	public bool RequestTile(Board board, Vector2Int position)
	{
		if (board.TileAt(position) != null) 
		{
			Debug.Log(board.TileAt(position));
			return false;
		}

		Surrounding surrounding = new Surrounding(position, board);
		
		Surrounding.State door = Surrounding.State.Door;
		if (surrounding.North != door && surrounding.East != door && surrounding.South != door && surrounding.West != door) 
		{
			Debug.Log("No doors");
			return false;
		}

		var tile = tilePool.GetRandomTile(board.TileChooser);
		if (tile == null) 
			return false;

		var newTile = Instantiate(tile.gameObject).GetComponent<Tile>();
		board.PutNewTile(position, newTile);

		var rotation = surrounding.NextValidRotation(newTile);
		newTile.Initialize(tileMeshPrefab, position, rotation);

		//NetworkServer.Spawn(newTile.gameObject);
		//RpcPutTile(newTile.gameObject, position, board.Signature);
		
		//newTile.OnDiscover();
		
		_currentSurrounding = surrounding;

		return true;
	}

	[ClientRpc(includeOwner = false)]
	void RpcPutTile(GameObject newTileObject, Vector2Int position, char boardSignature)
	{
		if (!this.isClientOnly) return;
		var newTile = newTileObject.GetComponent<Tile>();
		GetBoardBySignature(boardSignature).PutNewTile(position, newTile);
		newTile.Initialize(tileMeshPrefab, position, 0);
	}

	public Board GetBoardBySignature(char signature)
	{
		switch (signature)
		{
			case 'g': return groundBoard;
			case 'b': return basementBoard;
			case 'u': return upperBoard;
			default: throw new System.ArgumentException($"No board of '{signature}' signature");
		}
	}
}
