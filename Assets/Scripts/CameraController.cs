using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	private Camera _camera;

	[SerializeField]
	private PlayerInput _playerInput;

	[SerializeField]
	private float _moveDampRatio = 10;

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
		actions["zoom"].performed += OnZoom;
		actions["move"].performed += OnMove;
		actions["move"].canceled += OnStopMove;
		actions["MoveCamera"].performed += OnMoveCamera;
	}

	private void FixedUpdate() 
	{
		if (_isMoving) 
		{
			float x = _moveVector.x / _moveDampRatio;
			float z = _moveVector.y / _moveDampRatio;
			this.transform.Translate(x, 0, z, Space.World);
		}
	}
	
	public void OnZoom(InputAction.CallbackContext context)
	{
		float y = context.ReadValue<float>() / -120;
		_camera.fieldOfView += y;
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		_moveVector = context.ReadValue<Vector2>();
		_isMoving = true;
	}

	public void OnStopMove(InputAction.CallbackContext context)
	{
		_isMoving = false;
	}

	private void OnMoveCamera(InputAction.CallbackContext context)
	{
		var delta = context.ReadValue<Vector2>();
		delta /= -_moveDampRatio;
		Debug.Log(delta);
		this.transform.Translate(new Vector3(delta.x, 0, delta.y), Space.World);
	}
}
