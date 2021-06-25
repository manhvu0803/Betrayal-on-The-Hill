using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : NetworkBehaviour
{
    void Start()
    {
        if (!this.isServer) return;
		var roomManager = GameObject.FindObjectOfType<RoomManager>();
		var button = GetComponent<Button>();
		
		button.interactable = false;
		button.onClick.AddListener(roomManager.OnGameStart);
		
		// += : event delegate
		// () => code : anonymous function;
		roomManager.OnReady += () => { if (button != null) button.interactable = true; };
		roomManager.OnNotReady += () => { if (button != null) button.interactable = false; };
    }
}
