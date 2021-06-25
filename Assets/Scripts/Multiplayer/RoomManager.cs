using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : NetworkRoomManager
{
	[Header("Player settings")]
	[SerializeField] private string playerName;

	public delegate void VoidEvent();

	public event VoidEvent OnReady;
	public event VoidEvent OnNotReady;

	public override void OnStartClient()
	{
		base.OnStartClient();
		PlayerPrefs.SetString("playerName", playerName);
	}
	
	public override void OnStartHost()
	{
		base.OnStartHost();
		PlayerPrefs.SetString("playerName", playerName);
	}

	public override void OnRoomServerPlayersReady()
	{
		OnReady?.Invoke();
	}

	public override void OnRoomServerPlayersNotReady()
	{
		base.OnRoomServerPlayersNotReady();
		OnNotReady?.Invoke();
	}
	
	public void OnGameStart() => ServerChangeScene(base.GameplayScene);

	public void OnPlayerNameChanged(InputField nameField)
	{
		playerName = nameField.text;
	}
}
