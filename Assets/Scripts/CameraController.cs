using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Transform _highlighter;

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

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			// The raycast will hit the board mesh, which is a seperate GameObject, so we need to get its parent
			Board hitBoard = hit.transform.parent.GetComponent<Board>();
			Vector2Int boardPosition = hitBoard.GetTileFromWorldPoint(hit.point);
			Debug.Log($"{hitBoard}: {boardPosition}");
			_highlighter.transform.position = hitBoard.BoardPositionToWorld(boardPosition);
		}	
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
}
