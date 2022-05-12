using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class LocalController : MonoBehaviour
{
	[SerializeField] private double moveDelay;
	
	[SerializeField] GameObject highlighter;

	#region Boards
	[Header("Boards")]
	[SerializeField] private Board groundBoard;
	[SerializeField] private Board upperBoard;
	[SerializeField] private Board basementBoard;
	#endregion

	#region Events
	public event Action putTileEvent;
	public event Action<Vector2Int> moveEvent;
	public event Action<Board> switchBoardEvent;
	#endregion

	private Board currentBoard;

	private Vector2Int currentBoardSize;

	private Vector2Int cursorPosition;

	private Vector2Int movement;

	private bool allowRotate = false;

	private double lastMoveTime = 0;

    void Start()
	{
		upperBoard.gameObject.SetActive(false);
		basementBoard.gameObject.SetActive(false);
		SwitchBoard(groundBoard);
    }

	void Update()
	{
	}

	public void OnPutTile()
	{
		Debug.Log("OnPutTile");
		putTileEvent?.Invoke();
	}

	public void OnRotate(InputValue value)
	{
		if (!allowRotate) return;
	}

	public void OnReset()
	{
		Debug.Log("Reset game");
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
		
		currentBoardSize = new Vector2Int(board.Width - 1, board.Height - 1);
		cursorPosition = board.StartingPosition;
		
		switchBoardEvent?.Invoke(board);
	}
}
