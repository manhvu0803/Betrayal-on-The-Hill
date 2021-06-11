using Mirror;
using UnityEngine.UI;

public class BetrayalRoomManager : NetworkRoomManager
{
	private Button startButton;

	public override void OnRoomServerSceneChanged(string sceneName)
	{
		base.OnRoomServerSceneChanged(sceneName);
		if (sceneName == base.RoomScene) {
			foreach (var button in FindObjectsOfType<Button>()) {
				if (button.name.ToLower() == "startbutton") {
					startButton = button;
					startButton.interactable = false;
					startButton.onClick.AddListener(OnGameStart);
					break;
				}
			}
		}
	}

	public override void OnRoomServerPlayersReady()
	{
		if (startButton != null) startButton.interactable = true;
	}

	public override void OnRoomServerPlayersNotReady()
	{
		base.OnRoomServerPlayersNotReady();
		if (startButton != null) startButton.interactable = false;
	}

	public void OnGameStart()
	{
		ServerChangeScene(base.GameplayScene);
	}
}
