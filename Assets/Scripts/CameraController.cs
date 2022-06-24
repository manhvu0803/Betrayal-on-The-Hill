using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	private Camera _camera;

	[SerializeField]
	private PlayerInput _playerInput;

	private bool _isMoving = false;

	private Vector2 _moveVector;

	private void Start() 
	{
		_camera = this.GetComponent<Camera>();

		RegisterEvents();
	}

	private void RegisterEvents()
	{
		InputActionAsset actions = _playerInput.actions;
		actions["Zoom"].performed += OnZoom;
		actions["MoveCameraWithDirectional"].performed += OnMoveCameraWithDirectional;
		actions["MoveCameraWithDirectional"].canceled += OnStopMoveCameraWithDirectional;
		actions["MoveCameraWithPointer"].performed += OnMoveCameraWithPointer;
	}

	private void FixedUpdate() 
	{
		if (_isMoving) 
		{
			this.transform.Translate(_moveVector.x, 0, _moveVector.y, Space.World);
		}
	}
	
	public void OnZoom(InputAction.CallbackContext context)
	{
		float y = context.ReadValue<float>() / -120;
		_camera.fieldOfView += y;
	}

	public void OnMoveCameraWithDirectional(InputAction.CallbackContext context)
	{
		_moveVector = context.ReadValue<Vector2>();
		_isMoving = true;
	}

	public void OnStopMoveCameraWithDirectional(InputAction.CallbackContext context)
	{
		_isMoving = false;
	}

	private void OnMoveCameraWithPointer(InputAction.CallbackContext context)
	{
		var delta = context.ReadValue<Vector2>();
		this.transform.Translate(new Vector3(delta.x, 0, delta.y), Space.World);
	}
}
