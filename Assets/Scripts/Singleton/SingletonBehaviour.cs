using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : Component
{
	public static T Instance { get; private set; }

	protected virtual void Awake()
	{
		T thisInstance = this as T;

		if (Instance != null && Instance != thisInstance)
		{
			Debug.LogError($"There must be only 1 object of type {typeof(T).Name}");
			return;
		}

		Instance = thisInstance;
	}
}