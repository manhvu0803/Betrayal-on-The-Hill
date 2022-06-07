using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
	[SyncVar(hook = nameof(OnPlayerNameChanged))]
	public string PlayerName;

	[ReadOnlyField]
	public Board CurrentBoard;

	[ReadOnlyField, SyncVar] 
	public Vector2Int Position;

	

	void Start()
	{
		if (this.isLocalPlayer)
		{
			SetUpLocalPlayer();
		}
	}

	void SetUpLocalPlayer()
	{
		LocalController.Instance.OnSwitchBoard += SwitchBoard;
	}

	void SwitchBoard(Board board) => CmdSwitchBoard(board.Signature);

	[Command]
	void CmdSwitchBoard(char boardSignature)
	{
		CurrentBoard = GameManager.Instance.GetBoardBySignature(boardSignature);
		RpcSwitchBoard(boardSignature);
	}

	[ClientRpc]
	public void RpcSwitchBoard(char boardSignature) => CurrentBoard = GameManager.Instance.GetBoardBySignature(boardSignature);

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