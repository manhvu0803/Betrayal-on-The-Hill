using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
	private GameManager gameManager;

	private LocalController localController;

	[ReadOnlyField] public Board currentBoard;

	[ReadOnlyField] [SyncVar] public Vector2Int position;

	private new MeshRenderer renderer;

	void Start()
	{
		renderer = GetComponent<MeshRenderer>();

		if ((this.isLocalPlayer && this.isServer) || (!this.isLocalPlayer && this.isClientOnly)) gameObject.name = "RemotePlayer";
		else gameObject.name = "LocalPlayer";
		
		if (this.isLocalPlayer) LocalPlayerSetUp();
	}

	public override string ToString()
	{
		return $"{this.name}_{this.currentBoard}_{this.position}";
	}

	void LocalPlayerSetUp()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>();	
		localController = GameObject.FindObjectOfType<LocalController>();
		localController.putTileEvent += CmdPutTile;
		localController.moveEvent += CmdMove;
		localController.switchBoardEvent += SwitchBoard;
	}

	[Command]
	void CmdPutTile()
	{
		gameManager.RequestTile(this);
	}

	[Command]
	void CmdMove(Vector2Int pos)
	{
		position = pos;
	}

	void SwitchBoard(Board board)
	{
		CmdSwitchBoard(board.Signature);
	}
	
	[Command]
	void CmdSwitchBoard(char boardSignature)
	{
		gameManager.SwitchBoard(this, boardSignature);
	}
}