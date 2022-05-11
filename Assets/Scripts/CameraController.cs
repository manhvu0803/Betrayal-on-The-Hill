using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private float _moveDampRatio = 10;

	private bool _isMoving = false;
	private Vector2 _moveVector;

	public void OnZoom(InputAction.CallbackContext context)
	{
		if (!context.performed) 
			return;

		float y = context.ReadValue<float>() / -120;
		this.transform.Translate(0, y, 0, Space.World);
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		if (context.performed) 
		{	
			_moveVector = context.ReadValue<Vector2>();
			_isMoving = true;
		}
		else 
		{
			_isMoving = false;
		}
	}

	public void OnLeftClick(InputAction.CallbackContext context) 
	{
		if (!context.performed)
			return;

		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = _camera.ScreenPointToRay(mousePosition);

		Debug.Log(_camera.ScreenToWorldPoint(mousePosition));

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			Transform hitObject = hit.transform;
			Debug.Log(hitObject);
		}	}

	private void FixedUpdate() 
	{
		if (_isMoving) 
		{
			float x = _moveVector.x / _moveDampRatio;
			float z = _moveVector.y / _moveDampRatio;
			this.transform.Translate(x, 0, z, Space.World);
		}
	}
}
