using System;
using UnityEngine;
using Mirror;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class TilePool : MonoBehaviour
{
	private class TileItem
	{
		public Tile tile;

		public float weight;

		public bool used;

		public TileItem(Tile obj, float w = 0)
		{
			tile = obj;
			weight = w;
			used = false;
		}

		public override string ToString() => $"{tile} used:{used}";
	}

	private TileItem[] _tilePool;

	[SerializeField]
	private Tile[] _tileList;

	#if UNITY_EDITOR
	[ContextMenu("Get tile prefabs from Resources")]
	private void GetTileDataResources()
	{
		string[] directories = { "Assets/Prefabs/Tiles" };
		_tileList = Utilities.GetAsset<Tile>("t:prefab", directories).ToArray();
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}
	#endif

	private void Start()
	{
		_tilePool = new TileItem[_tileList.Length];
		
		for (int i = 0; i < _tileList.Length; ++i) 
		{
			//NetworkClient.RegisterPrefab(tile.gameObject);
			_tilePool[i] = new TileItem(_tileList[i], UnityEngine.Random.Range(0, 50));
		}
		Array.Sort(_tilePool, (a, b) => a.weight.CompareTo(b.weight));
	}

	public void ResetPool()
	{
		foreach (TileItem item in _tilePool) 
		{
			item.tile.gameObject.SetActive(false);
			item.used = false;
			item.weight = UnityEngine.Random.Range(0, 50);
		}

		Array.Sort(_tilePool, (a, b) => a.weight.CompareTo(b.weight));
	}

	public Tile GetRandomTile()
	{
		int i = 0;
		while (i < _tilePool.Length && _tilePool[i].used)
			++i;

		if (_tilePool.Length <= i) {
			Debug.Log("Out of tiles");
			return null;
		}
		_tilePool[i].used = true;
		_tilePool[i].tile.gameObject.SetActive(true);
		return _tilePool[i].tile;
	}

	public Tile GetRandomTile(Func<TileLocation, bool> chooser)
	{
		int i = 0;
		while (i < _tilePool.Length && (_tilePool[i].used || !chooser(_tilePool[i].tile.Location)))
			++i;

		if (i >= _tilePool.Length) {
			Debug.Log($"Out of tiles for {chooser.Target}");
			return null;
		}

		_tilePool[i].used = true;
		_tilePool[i].tile.gameObject.SetActive(true);
		return _tilePool[i].tile;
	}
}
