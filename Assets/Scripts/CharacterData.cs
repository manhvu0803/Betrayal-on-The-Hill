using UnityEngine;

[CreateAssetMenu(menuName = "Betrayal/Character data")]
public class CharacterData : ScriptableObject
{
	public string Name;
	
	[Header("Starting stat level")]
	public int SpeedLevel;
	
	public int MightLevel;
	
	public int SanityLevel;
	
	public int KnowledgeLevel;

	[Header("Stat chart")]
	public int[] SpeedChart;
	
	public int[] MightChart;
	
	public int[] SanityChart;
	
	public int[] KnowledgeChart;
}
