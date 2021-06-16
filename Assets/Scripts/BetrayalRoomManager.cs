using Mirror;
using UnityEngine.UI;

public class BetrayalRoomManager : NetworkRoomManager
{
	public delegate void VoidEvent();

	public event VoidEvent OnReady;
	public event VoidEvent OnNotReady;

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
}
