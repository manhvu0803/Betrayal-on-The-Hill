using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomManager : NetworkRoomManager
{
	[Header("Player settings")]
	[SerializeField]
	private string playerName;

	public event Action OnReady;
	public event Action OnNotReady;

	private new void Start()
	{
		playerName = PlayerPrefs.GetString("playerName");
	}

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

	// public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection connection, GameObject roomPlayer, GameObject gamePlayer)
	// {
	// 	gamePlayer.GetComponent<NetworkPlayer>().playerName = playerName;
	// 	return true;
	// }
	
	public void OnGameStart() => base.OnRoomServerPlayersReady();

	public void OnPlayerNameChanged(InputField nameField)
	{
		playerName = nameField.text;
	}
}
