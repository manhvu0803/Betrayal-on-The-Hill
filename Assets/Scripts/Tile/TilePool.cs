using System;
using UnityEngine;
using Mirror;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class TilePool : Pool<Tile>
{
	#if UNITY_EDITOR
	[ContextMenu("Get tile prefabs from Assets/Prefabs/Tiles")]
	private void GetTileDataResources()
	{
		string[] directories = { "Assets/Prefabs/Tiles" };
		_itemList = Utilities.GetAsset<Tile>("t:prefab", directories).ToArray();
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}
	#endif
}
