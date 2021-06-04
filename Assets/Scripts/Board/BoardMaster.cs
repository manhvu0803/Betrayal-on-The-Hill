using UnityEngine;

public class BoardMaster : MonoBehaviour
{
	public TilePool tilePool;
	public Board groundBoard 	{ get; private set; }
	public Board upperBoard 	{ get; private set; }
	public Board basementBoard 	{ get; private set; }

	void Start()
	{
		groundBoard = GetComponentInChildren<GroundBoard>();
		upperBoard = GetComponentInChildren<UpperBoard>();
		basementBoard = GetComponentInChildren<BasementBoard>();
	}

	public void Reset()
	{
		groundBoard.Reset();
		//upperBoard.Reset();
		//basementBoard.Reset();
		tilePool.Reset();
		Debug.Log("Reset");
	}
}