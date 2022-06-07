using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class LocalController : SingletonBehaviour<LocalController>
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Transform _highlighter;

	[SerializeField]
	private GameManager _gameManager;

	#region Boards
	[Header("Boards")]
	[SerializeField] 
	private Board _groundBoard;

	[SerializeField] 
	private Board _upperBoard;
	
	[SerializeField] 
	private Board _basementBoard;
	#endregion

	#region Events
	public event Action<Board> OnSwitchBoard;

	public event Action OnStartPuttingTile;

	public event Action OnEndPuttingTile;
	#endregion

	private Board _currentBoard;

	private Vector2Int _currentPosition;

	// Certain actions (ie. a left click) invoke both Player events and UI buttons,
	// and Unity discourage calling the check function in InputAction callbacks,
	// so we update this variable in Update() instead
	private bool _pointerIsOnUi;

    void Start()
	{
		_upperBoard.gameObject.SetActive(false);
		_basementBoard.gameObject.SetActive(false);
		SwitchBoard(_groundBoard);

		PlayerInput playerInput = this.GetComponent<PlayerInput>();
		playerInput.actions["SelectTile"].performed += OnLeftClick;
    }

	void Update()
	{
		_pointerIsOnUi = EventSystem.current.IsPointerOverGameObject();
	}

	public void SwitchBoard(Board board)
	{
		if (board == _currentBoard) return;

		_currentBoard?.gameObject.SetActive(false);
		_currentBoard = board;
		_currentBoard.gameObject.SetActive(true);

		OnSwitchBoard?.Invoke(board);

		_currentPosition = _currentBoard.StartPosition;
		_camera.transform.position = _currentBoard.BoardPositionToWorld(_currentPosition, _camera.transform.position.y);
		MoveCursorTo(_currentPosition);
	}

	public void OnLeftClick(InputAction.CallbackContext context)
	{
		if (_pointerIsOnUi)
			return;

		OnEndPuttingTile?.Invoke();
		UpdateCursorPostion();
	}

	private void UpdateCursorPostion()
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = _camera.ScreenPointToRay(mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			// The raycast will hit the board mesh, which is a seperate GameObject, so we need to get its parent
			Board hitBoard = hit.transform.parent.GetComponent<Board>();
			_currentPosition = hitBoard.GetSquareFromWorldPoint(hit.point);
			MoveCursorTo(_currentPosition);
		}
	}

	private void MoveCursorTo(Vector2Int position)
	{
		_highlighter.transform.position = _currentBoard.BoardPositionToWorld(position);
	}

	public void PlaceTile()
	{
		if (!_gameManager.RequestTile(_currentBoard, _currentPosition))
			return;

		OnStartPuttingTile?.Invoke();
	}

	public void RotateTile()
	{
		//_currentBoard.RotateTile(_currentPosition, 1);
	}
}
