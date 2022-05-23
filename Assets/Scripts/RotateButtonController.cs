using UnityEngine;
using UnityEngine.UI;

public class RotateButtonController : MonoBehaviour
{
	[SerializeField]
	private LocalController _localController;

	void Start()
	{
		Button button = this.GetComponent<Button>();

		button.interactable = false;

		_localController.OnStartPuttingTile += () => button.interactable = true;
		_localController.OnEndPuttingTile += () => button.interactable = false;
	}
}
