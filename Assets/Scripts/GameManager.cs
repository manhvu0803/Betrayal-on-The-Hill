using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : SingletonNetBehaviour<GameManager>
{
    [SerializeField] private GameObject tileMeshPrefab;

    [SerializeField] private TilePool tilePool;

    [Header("Game boards")]
    [SerializeField] 
    private Board groundBoard;
    
    [SerializeField] 
    private Board upperBoard;
    
    [SerializeField] 
    private Board basementBoard;

    [ReadOnlyField, SerializeField] 
	private List<NetworkPlayer> players;

    private Surrounding _currentSurrounding = null;

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    [Server]
    public bool RequestTile(Board board, Vector2Int position)
    {
        if (board.TileAt(position) != null)
        {
            Debug.Log(board.TileAt(position));
            return false;
        }

        Surrounding surrounding = new Surrounding(position, board);

        if (surrounding.DoorCount <= 0)
        {
            Debug.Log("No surrounding doors");
            return false;
        }

        var tile = tilePool.GetRandomItemBy(board.TileChooser);
        if (tile == null)
            return false;

        var newTile = Instantiate(tile.gameObject).GetComponent<Tile>();
        board.PutNewTile(position, newTile);

        newTile.Initialize(tileMeshPrefab, position);
        newTile.transform.Rotate(0, 0, surrounding.NextValidRotation(newTile));

        //NetworkServer.Spawn(newTile.gameObject);
        //RpcPutTile(newTile.gameObject, position, board.Signature);

        _currentSurrounding = surrounding;
        newTile.OnDiscover();

        return true;
    }

    [ClientRpc(includeOwner = false)]
    void RpcPutTile(GameObject newTileObject, Vector2Int position, Board board)
    {
        if (!this.isClientOnly) return;
        var newTile = newTileObject.GetComponent<Tile>();
        board.PutNewTile(position, newTile);
        newTile.Initialize(tileMeshPrefab, position);
    }

    public void RegisterPlayer(NetworkPlayer player)
    {
        players.Add(player);
    }
}
