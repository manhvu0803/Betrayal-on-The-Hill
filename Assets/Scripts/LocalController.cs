using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class LocalController : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private Transform _highlighter;

	[SerializeField]
	private GameManager _gameManager;

	private PlayerInput _playerInput;

	#region Boards
	[Header("Boards")]
	[SerializeField] private Board groundBoard;
	[SerializeField] private Board upperBoard;
	[SerializeField] private Board basementBoard;
	#endregion

	#region Events
	public event Action<Board> switchBoardEvent;
	#endregion

	private Board _currentBoard;

	[SerializeField]
	private Vector2Int _currentPosition;

	// Certain actions (ie. a left click) invoke both Player events and UI buttons,
	// and Unity discourage calling the check function in InputAction callbacks,
	// so we update this variable in Update()
	private bool _pointerIsOnUi;

    void Start()
	{
		upperBoard.gameObject.SetActive(false);
		basementBoard.gameObject.SetActive(false);
		SwitchBoard(groundBoard);

		_playerInput = this.GetComponent<PlayerInput>();
		_playerInput.actions["SelectTile"].performed += OnSelectTile;
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
		
		switchBoardEvent?.Invoke(board);

		Vector2Int startPosition = _currentBoard.StartPosition;
		_camera.transform.position = _currentBoard.BoardPositionToWorld(startPosition, _camera.transform.position.y);
	}
	
	public void OnSelectTile(InputAction.CallbackContext context)
	{
		if (_pointerIsOnUi)
			return;

		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = _camera.ScreenPointToRay(mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			// The raycast will hit the board mesh, which is a seperate GameObject, so we need to get its parent
			Board hitBoard = hit.transform.parent.GetComponent<Board>();
			_currentPosition = hitBoard.GetTileFromWorldPoint(hit.point);
			Debug.Log(_currentPosition);
			_highlighter.transform.position = hitBoard.BoardPositionToWorld(_currentPosition);
		}	
	}

	public void OnPlaceTile()
	{
		_gameManager.RequestTile(_currentBoard, _currentPosition);
	}
}
