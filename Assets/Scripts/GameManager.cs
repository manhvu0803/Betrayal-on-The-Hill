using Mirror;
using UnityEngine;
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

	private List<NetworkPlayer> players;

	public void Start()
	{
		if (tilePool == null) throw new System.NullReferenceException("tilePool is null");

		players = new List<NetworkPlayer>(GameObject.FindObjectsOfType<NetworkPlayer>());

		foreach (var player in players) {
			player.currentBoard = groundBoard;
			player.position = groundBoard.StartingPosition;
		}
	}
	
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

	public void SwitchBoard(NetworkPlayer player, char boardSignature)
	{
		player.currentBoard = GetBoardBySignature(boardSignature);
		RpcSwitchBoard(player.netId, boardSignature);
	}

	[ClientRpc(includeOwner = false)]
	void RpcPutTile(GameObject newTileObject, Vector2Int pos, char boardSignature)
	{
		if (!this.isClientOnly) return;
		var newTile = newTileObject.GetComponent<Tile>();
		newTile.Initialize(tileMeshPrefab);
		GetBoardBySignature(boardSignature).PutNewTile(pos, newTile);
	}

	[ClientRpc]
	void RpcSwitchBoard(uint playerId, char boardSignature)
	{
		GetPlayerByNetId(playerId).currentBoard = GetBoardBySignature(boardSignature);
	}

	private Board GetBoardBySignature(char signature)
	{
		switch (signature)
		{
			case 'g': return groundBoard;
			case 'b': return basementBoard;
			case 'u': return upperBoard;
			default: throw new System.ArgumentException($"No board of '{signature}' signature");
		}
	}

	private NetworkPlayer GetPlayerByNetId(uint netId)
	{
		foreach (var player in players)
			if (player.netId == netId) return player;
		return null;
	}
}
