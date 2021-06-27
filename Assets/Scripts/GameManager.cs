using Mirror;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SurState = Board.Surrounding.State;

public class GameManager : NetworkBehaviour
{
	[SerializeField] private GameObject tileMeshPrefab;

	[SerializeField] private TilePool tilePool;

	[Header("Game boards")]
	[SerializeField] private Board groundBoard;
	[SerializeField] private Board upperBoard;
	[SerializeField] private Board basementBoard;

	[ReadOnlyField] [SerializeField] private List<NetworkPlayer> players;

	public override void OnStartServer()
	{
		base.OnStartServer();
		if (tilePool == null) throw new System.NullReferenceException("tilePool is null");
		StartCoroutine(GetPlayers(GameObject.FindObjectOfType<NetworkRoomManager>().numPlayers));
	}

	IEnumerator GetPlayers(int playerCount)
	{
		// Polling every 2 frames until found all player object
		// Since the build keep failing to find remote player
		while (players.Count < playerCount) {
			yield return null;
			yield return null;
			players = new List<NetworkPlayer>(GameObject.FindObjectsOfType<NetworkPlayer>());
		}

		foreach (var player in players) {
			Debug.Log(player);
			player.currentBoard = groundBoard;
			player.position = groundBoard.StartingPosition;
		}
	}
	
	[Server]
	public void RequestTile(NetworkPlayer player)
	{
		var board = player.currentBoard;
		var pos = player.position;

		if (board.TileAt(pos) != null) {
			Debug.Log(board.TileAt(pos));
			return;
		}

		var surrounding = board.GetSurrounding(pos);
		if (surrounding.north != SurState.Door 
		 && surrounding.east  != SurState.Door 
		 && surrounding.south != SurState.Door 
		 && surrounding.west  != SurState.Door) {
			Debug.Log("No doors");
			return;
		}

		var tile = tilePool.GetRandomTile(board.TileChooser);
		if (tile == null) return;

		var newTile = Instantiate(tile.gameObject).GetComponent<Tile>();
		board.PutNewTile(pos, newTile);

		var rotation = Board.NextValidRotation(newTile, surrounding, 1);
		newTile.Initialize(tileMeshPrefab, pos, rotation);

		NetworkServer.Spawn(newTile.gameObject);
		RpcPutTile(newTile.gameObject, pos, board.Signature);
		
		newTile.OnDiscover();
	}

	[ClientRpc(includeOwner = false)]
	void RpcPutTile(GameObject newTileObject, Vector2Int pos, char boardSignature)
	{
		if (!this.isClientOnly) return;
		var newTile = newTileObject.GetComponent<Tile>();
		GetBoardBySignature(boardSignature).PutNewTile(pos, newTile);
		newTile.Initialize(tileMeshPrefab, pos, 0);
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
