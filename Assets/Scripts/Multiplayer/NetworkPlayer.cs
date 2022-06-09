using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
	[ReadOnlyField, SyncVar(hook = nameof(OnPlayerNameChanged))]
	public string PlayerName;

	[ReadOnlyField]
	public Board CurrentBoard;

	[ReadOnlyField, SyncVar] 
	public Vector2Int Position;

	[SerializeField, ReadOnlyField]
	private CharaterData CharaterData;
	
	void Start()
	{
		if (this.isLocalPlayer)
		{
			SetUpLocalPlayer();
		}
	}

	void SetUpLocalPlayer()
	{
		LocalController.Instance.OnSwitchBoard += CmdSwitchBoard;
	}

	[Command]
	void CmdSwitchBoard(Board board)
	{
		RpcSwitchBoard(board);
	}

	[ClientRpc]
	public void RpcSwitchBoard(Board board) => CurrentBoard = board;

	void OnPlayerNameChanged(string oldName, string newName)
	{
		Debug.Log($"Player {oldName} change to {newName}");
		this.gameObject.name = $"{newName}_{((this.isLocalPlayer)? "Local" : "Remote")}";
	}

	public override string ToString()
	{
		return $"{name}_{CurrentBoard}_{Position}";
	}
}