using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
	[SerializeField] private GameObject highlighter;

	private Vector2Int movement;

	[SerializeField] private double moveDelay;
	
	[SerializeField] private BoardMaster boardMaster;

	private Board currentBoard;

	private bool allowRotate = false;

	private double lastMoveTime = 0;

    void Start()
    {	
		highlighter.transform.localScale = this.transform.lossyScale;
		StartCoroutine(LateStart());
    }

	IEnumerator LateStart()
	{
		yield return new WaitForEndOfFrame();
		currentBoard = boardMaster.groundBoard;
		MoveHighlighter();
	}

	void Update()
	{
		lastMoveTime += Time.deltaTime;
		if (movement != Vector2Int.zero && lastMoveTime >= moveDelay) {
			lastMoveTime = 0;
			allowRotate = false;
			currentBoard.MovePlayer(movement);
			MoveHighlighter();
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		movement = Vector2Int.RoundToInt(context.ReadValue<Vector2>());
	}

	public void OnPutTile(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (currentBoard.CurrentTile() == null) {
			currentBoard.PutNewTile();
			allowRotate = true;
			return;
		}
	}

	public void OnRotate(InputAction.CallbackContext context)
	{
		if (!context.performed || !allowRotate) return;
		currentBoard.Rotate((int)context.ReadValue<float>());
	}

	public void OnReset()
	{
		boardMaster.Reset();
	}

	public void OnSwitchGround()
	{
		currentBoard = boardMaster.groundBoard;
		MoveHighlighter();
	}

	public void OnSwitchUpper()
	{
		currentBoard = boardMaster.upperBoard;
		MoveHighlighter();
	}

	public void OnSwitchBasement()
	{
		currentBoard = boardMaster.basementBoard;
		MoveHighlighter();
	}

	private void MoveHighlighter()
	{
		var trf = highlighter.transform;
		trf.position = currentBoard.CurrentPositionToWorld(trf.position.y);
	}
}