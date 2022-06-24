using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomManager : NetworkRoomManager
{
	[SerializeField]
	private CharacterData[] _characterList;

	public event Action OnReady;
	public event Action OnNotReady;

	public override void OnStartClient()
	{
		base.OnStartClient();
	}
	
	public override void OnStartHost()
	{
		base.OnStartHost();
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
}
