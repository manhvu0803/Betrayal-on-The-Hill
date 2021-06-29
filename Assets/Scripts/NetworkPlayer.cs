using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
	private GameManager gameManager;

	private LocalController localController;

	[SyncVar(hook = nameof(ChangePlayerName))] public string playerName;

	[ReadOnlyField] public Board currentBoard;

	[ReadOnlyField] [SyncVar] public Vector2Int position;

#if UNITY_EDITOR
	private new MeshRenderer renderer;
#else
	private MeshRenderer renderer;
#endif

	void Start()
	{
		gameManager = GameObject.FindObjectOfType<GameManager>();

		renderer = GetComponent<MeshRenderer>();

		if (this.isLocalPlayer) LocalPlayerSetUp();
	}

	public override string ToString()
	{
		return $"{this.name}_{this.currentBoard}_{this.position}";
	}

	void LocalPlayerSetUp()
	{
		localController = GameObject.FindObjectOfType<LocalController>();
		localController.putTileEvent += CmdPutTile;
		localController.moveEvent += CmdMove;
		localController.switchBoardEvent += SwitchBoard;
	}

	[Command]
	void CmdPutTile() => gameManager.RequestTile(this);

	[Command]
	void CmdMove(Vector2Int pos) => position = pos;

	void SwitchBoard(Board board) => CmdSwitchBoard(board.Signature);

	[Command]
	void CmdSwitchBoard(char boardSignature)
	{
		this.currentBoard = gameManager.GetBoardBySignature(boardSignature);
		this.RpcSwitchBoard(boardSignature);
	}

	[ClientRpc]
	public void RpcSwitchBoard(char boardSignature) => this.currentBoard = gameManager.GetBoardBySignature(boardSignature);

	void ChangePlayerName(string oldName, string newName)
	{
		Debug.Log($"Player {oldName} change to {newName}");
		this.gameObject.name = $"{newName}_{((this.isLocalPlayer)? "Local" : "Remote")}";
	}
}