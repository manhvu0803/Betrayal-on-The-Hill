using UnityEngine;

public class OmenPool : Pool<Omen>
{
	private void Start()
	{
		Debug.Log(TilePool.Instance);
	}
}
