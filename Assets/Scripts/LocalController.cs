using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class LocalController : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Transform _highlighter;

	[SerializeField]
	private GameManager _gameManager;

	#region Boards
	[Header("Boards")]
	[SerializeField] private Board groundBoard;
	[SerializeField] private Board upperBoard;
	[SerializeField] private Board basementBoard;
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
		upperBoard.gameObject.SetActive(false);
		basementBoard.gameObject.SetActive(false);
		SwitchBoard(groundBoard);

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
		MoveCursorToCurrentPosition();
	}

	public void OnLeftClick(InputAction.CallbackContext context)
	{
		if (_pointerIsOnUi)
			return;

		OnEndPuttingTile?.Invoke();
		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = _camera.ScreenPointToRay(mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			// The raycast will hit the board mesh, which is a seperate GameObject, so we need to get its parent
			Board hitBoard = hit.transform.parent.GetComponent<Board>();
			_currentPosition = hitBoard.GetSquareFromWorldPoint(hit.point);
			MoveCursorToCurrentPosition();
		}
	}

	private void MoveCursorToCurrentPosition()
	{
		_highlighter.transform.position = _currentBoard.BoardPositionToWorld(_currentPosition);
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
