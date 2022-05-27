using System;
using UnityEngine;

public class Pool<T> : SingletonBehaviour where T : MonoBehaviour
{
    protected class PoolItem
    {
		public static readonly Comparison<PoolItem> Comparator = (a, b) => a.Weight.CompareTo(b.Weight);

		public T Item;

		public float Weight;

		public bool Used;

		public PoolItem(T obj)
		{
			Item = obj;
			Weight = UnityEngine.Random.value;
			Used = false;
		}

		public void Reset()
		{
			Weight = UnityEngine.Random.value;
			Used = false;
			Item.gameObject.SetActive(false);
		}

		public void Activate()
		{
			Used = true;
			Item.gameObject.SetActive(true);
		}

		public override string ToString() => $"{Item} used:{Used}";
    }

	protected PoolItem[] _itemPool;

	[SerializeField]
	protected T[] _itemList;

	protected override void Start()
	{
		base.Start();
		
		_itemPool = new PoolItem[_itemList.Length];
		
		for (int i = 0; i < _itemList.Length; ++i) 
		{
			//NetworkClient.RegisterPrefab(tile.gameObject);
			_itemPool[i] = new PoolItem(_itemList[i]);
		}
		Array.Sort(_itemPool, PoolItem.Comparator);
	}

	public void ResetPool()
	{
		foreach (PoolItem item in _itemPool) 
		{
			item.Reset();
		}

		Array.Sort(_itemPool, PoolItem.Comparator);
	}
	
	public T GetRandomItem()
	{
		int i = 0;
		while (i < _itemPool.Length && _itemPool[i].Used)
			++i;

		if (_itemPool.Length <= i) 
		{
			Debug.Log($"Out of item for {this}");
			return null;
		}

		_itemPool[i].Activate();
		return _itemPool[i].Item;
	}

	public T GetRandomItemBy(Func<T, bool> chooser)
	{
		int i = 0;
		while (i < _itemPool.Length && (_itemPool[i].Used || !chooser(_itemPool[i].Item)))
			++i;

		if (i >= _itemPool.Length) {
			Debug.Log($"Out of tiles for {chooser.Target}");
			return null;
		}

		_itemPool[i].Activate();
		return _itemPool[i].Item;
	}
}
