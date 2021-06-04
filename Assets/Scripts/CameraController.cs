using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	public void OnZoom(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		this.transform.Translate(0, context.ReadValue<float>() / -120, 0, Space.World);
	}
}
