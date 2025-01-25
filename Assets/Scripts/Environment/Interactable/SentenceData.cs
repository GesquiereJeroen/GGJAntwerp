using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Sentence Data")]
public class SentenceData : ScriptableObject
{
	#region Editor Fields
	[SerializeField] private List<string> _beginning = new List<string>();
	[SerializeField] private List<string> _middle = new List<string>();
	[SerializeField] private List<string> _ending = new List<string>();
	[SerializeField] private List<string> _conclusion = new List<string>();
	#endregion

	#region Properties
	public List<string> Beginning => _beginning;
	public List<string> Middle => _middle;
	public List<string> Ending => _ending;
	public List<string> Conclusion => _conclusion;
	#endregion

	#region Methods
	public string GetRandomBeginning()
	{
		int randIndex = Random.Range(0, Beginning.Count);

		return Beginning[randIndex];
	}
	public string GetRandomMiddle()
	{
		int randIndex = Random.Range(0, Middle.Count);

		return Middle[randIndex];
	}
	public string GetRandomEnding()
	{
		int randIndex = Random.Range(0, Ending.Count);

		return Ending[randIndex];
	}
	public string GetRandomConclusion()
	{
		int randIndex = Random.Range(0, Conclusion.Count);

		return Conclusion[randIndex];
	}
	#endregion
}
