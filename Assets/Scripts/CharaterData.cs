using UnityEngine;

[CreateAssetMenu(menuName = "My assets/Character data")]
public class CharaterData : ScriptableObject
{
	public string Name;
	
	[Header("Stats")]
	public int Speed;
	
	public int Might;
	
	public int Sanity;
	
	public int Knowledge;
}
