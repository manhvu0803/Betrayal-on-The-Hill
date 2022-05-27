using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Utilities
{
	public static List<T> GetAsset<T>(string filter, string[] directoriesToSearch) where T : Object
	{
		string[] guids = AssetDatabase.FindAssets(filter, directoriesToSearch);

		List<T> assetList = new();
		foreach (string guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			T asset = AssetDatabase.LoadAssetAtPath<T>(path);
			if (asset != null)
			{
				assetList.Add(asset);
			}
		}

		Debug.Log($"Loaded {assetList.Count} assets");
		return assetList;
	}
}
