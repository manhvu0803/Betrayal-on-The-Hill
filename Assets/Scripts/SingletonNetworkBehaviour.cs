using System;
using Mirror;

public class SingletonNetworkBehaviour : NetworkBehaviour
{
	private static SingletonNetworkBehaviour _instance = null;

	public static SingletonNetworkBehaviour Instance { get; }

    protected virtual void Start()
    {
		if (_instance != null)
		{
			throw new Exception($"There must be only 1 {this.GetType().Name} in a scene");
		}

		_instance = this;
    }
}
