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
		lastMoveTime += Time.deltaTime;
		if (movement != Vector2Int.zero && lastMoveTime >= moveDelay) {
			lastMoveTime = 0;
			allowRotate = false;
			cursorPosition += movement;
			cursorPosition.Clamp(Vector2Int.zero, currentBoardSize);
			
			MoveHighlighter();
			moveEvent?.Invoke(cursorPosition);
		}
	}

	public void OnMove(InputValue value)
	{
		movement = Vector2Int.RoundToInt(value.Get<Vector2>());
	}

	public void OnPutTile()
	{
		Debug.Log("OnPutTile");
		putTileEvent?.Invoke();
	}

	public void OnRotate(InputValue value)
	{
		if (!allowRotate) return;
		//currentBoard.Rotate((int)context.ReadValue<float>());
	}

	public void OnReset()
	{
		Debug.Log("Reset game");
	}

	public void OnSwitchGround() 	=> SwitchBoard(groundBoard);
	public void OnSwitchUpper() 	=> SwitchBoard(upperBoard);
	public void OnSwitchBasement() 	=> SwitchBoard(basementBoard);

	private void SwitchBoard(Board board)
	{
		if (board == currentBoard) return;

		currentBoard?.gameObject.SetActive(false);
		currentBoard = board;
		currentBoard.gameObject.SetActive(true);
		
		currentBoardSize = new Vector2Int(board.Width - 1, board.Height - 1);
		cursorPosition = board.StartingPosition;
		
		MoveHighlighter();
		switchBoardEvent?.Invoke(board);
	}

	private void MoveHighlighter() => StartCoroutine(MoveAtFrameEnd());
	
	IEnumerator MoveAtFrameEnd()
	{
		yield return new WaitForEndOfFrame();
		var transf = highlighter.transform;
		transf.position = currentBoard.BoardPositionToWorld(cursorPosition, transf.position.y);
	}
}
