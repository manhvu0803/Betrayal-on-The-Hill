using UnityEngine;
using UnityEngine.UI;

public class NameField : MonoBehaviour
{
	void Start()
	{
		GetComponent<InputField>().text = PlayerPrefs.GetString("playerName", "");
	}
}
