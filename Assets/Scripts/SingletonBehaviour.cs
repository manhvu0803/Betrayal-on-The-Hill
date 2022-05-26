using System;
using UnityEngine;

public class SingletonBehaviour : MonoBehaviour
{
	private static SingletonBehaviour _instance = null;

	public static SingletonBehaviour Instance { get; }

    protected virtual void Start()
    {
		if (_instance != null)
		{
			throw new Exception($"There must be only 1 {this.GetType().Name} in a scene");
		}

		_instance = this;
    }
}
