using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(PlayerInput))]
public class LocalController : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;

	[SerializeField]
	Transform _highlighter;

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

	private Board currentBoard;

    void Start()
	{
		upperBoard.gameObject.SetActive(false);
		basementBoard.gameObject.SetActive(false);
		SwitchBoard(groundBoard);

		_playerInput = this.GetComponent<PlayerInput>();
		_playerInput.actions["SelectTile"].performed += OnSelectTile;
    }

	public void OnSwitchGround() => SwitchBoard(groundBoard);
	public void OnSwitchUpper() => SwitchBoard(upperBoard);
	public void OnSwitchBasement() 	=> SwitchBoard(basementBoard);

	private void SwitchBoard(Board board)
	{
		if (board == currentBoard) return;

		currentBoard?.gameObject.SetActive(false);
		currentBoard = board;
		currentBoard.gameObject.SetActive(true);
		
		switchBoardEvent?.Invoke(board);
	}
	
	public void OnSelectTile(InputAction.CallbackContext context) 
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = _camera.ScreenPointToRay(mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			// The raycast will hit the board mesh, which is a seperate GameObject, so we need to get its parent
			Board hitBoard = hit.transform.parent.GetComponent<Board>();
			Vector2Int boardPosition = hitBoard.GetTileFromWorldPoint(hit.point);
			_highlighter.transform.position = hitBoard.BoardPositionToWorld(boardPosition);
		}	
	}
}
