using System.Collections.Generic;
using System;
using UnityEngine;

public class TilePool : MonoBehaviour
{
	class TileItem
	{
		public Tile tile;

		public float weight;

		public bool isUsed;

		public TileItem(Tile obj, float w = 0)
		{
			tile = obj;
			weight = w;
			isUsed = false;
		}

		public override string ToString()
		{
			return $"{tile} used:{isUsed}";
		}
	}

	private List<TileItem> tilePool;

	private void Start()
	{
		tilePool = new List<TileItem>();
		
		List<Tile> tileList = new List<Tile>(GetComponentsInChildren<Tile>());
		foreach (var tile in tileList) {
			tile.gameObject.SetActive(false);
			tilePool.Add(new TileItem(tile, UnityEngine.Random.Range(0, 50)));
		}
		tilePool.Sort((a, b) => a.weight.CompareTo(b.weight));
	}

	/*
	[ContextMenu("Get tile data")]
	private void GetTileDataResources()
	{
		tileList = new List<TileData>(Resources.LoadAll<TileData>("MapTiles/"));
		Debug.Log($"Get {tileList.Count} tiles' data in Resources");
	}
	*/

	public void Reset()
	{
		foreach (var item in tilePool) {
			item.tile.gameObject.SetActive(false);
			item.isUsed = false;
			item.weight = UnityEngine.Random.Range(0, 50);
		}
		tilePool.Sort((a, b) => a.weight.CompareTo(b.weight));
	}

	public Tile GetTile()
	{
		int i = 0;
		while (i <= tilePool.Count && tilePool[i].isUsed)
			++i;

		if (tilePool.Count <= i) {
			Debug.Log("Out of tiles");
			return null;
		}
		
		tilePool[i].isUsed = true;
		tilePool[i].tile.gameObject.SetActive(true);
		return tilePool[i].tile;
	}

	public Tile GetTile(Func<TileData.Location, bool> getter)
	{
		int i = 0;
		while (i < tilePool.Count && (tilePool[i].isUsed || !getter(tilePool[i].tile.GetLocation())))
			++i;

		if (i >= tilePool.Count) {
			Debug.Log($"Out of tiles for {getter.Target}");
			return null;
		}

		tilePool[i].isUsed = true;
		tilePool[i].tile.gameObject.SetActive(true);
		return tilePool[i].tile;
	}
}
