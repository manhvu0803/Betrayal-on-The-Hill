using System;
using System.Collections.Generic;
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

	private const string tileDir = "TilePrefabs/";

	private List<TileItem> tilePool;

	[SerializeField] private List<Tile> tileList;

	[ContextMenu("Get tile prefabs from Resources")]
	private void GetTileDataResources()
	{
		#if UNITY_EDITOR
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		#endif

		tileList = new List<Tile>(Resources.LoadAll<Tile>(tileDir));
		Debug.Log($"Loaded {tileList.Count} tiles in Resources/{tileDir}");
	}

	private void Start()
	{
		tilePool = new List<TileItem>();
		
		foreach (var tile in tileList) {
			//NetworkClient.RegisterPrefab(tile.gameObject);
			tilePool.Add(new TileItem(tile, UnityEngine.Random.Range(0, 50)));
		}
		tilePool.Sort((a, b) => a.weight.CompareTo(b.weight));
	}

	public void Reset()
	{
		foreach (var item in tilePool) {
			item.tile.gameObject.SetActive(false);
			item.used = false;
			item.weight = UnityEngine.Random.Range(0, 50);
		}
		tilePool.Sort((a, b) => a.weight.CompareTo(b.weight));
	}

	public Tile GetRandomTile()
	{
		int i = 0;
		while (i < tilePool.Count && tilePool[i].used)
			++i;

		if (tilePool.Count <= i) {
			Debug.Log("Out of tiles");
			return null;
		}
		tilePool[i].used = true;
		tilePool[i].tile.gameObject.SetActive(true);
		return tilePool[i].tile;
	}

	public Tile GetRandomTile(Func<TileLocation, bool> chooser)
	{
		int i = 0;
		while (i < tilePool.Count && (tilePool[i].used || !chooser(tilePool[i].tile.Location)))
			++i;

		if (i >= tilePool.Count) {
			Debug.Log($"Out of tiles for {chooser.Target}");
			return null;
		}

		tilePool[i].used = true;
		tilePool[i].tile.gameObject.SetActive(true);
		return tilePool[i].tile;
	}
}
