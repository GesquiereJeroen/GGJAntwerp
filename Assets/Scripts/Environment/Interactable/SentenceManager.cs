using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SentenceManager : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private SentenceData _sentenceData;
	[SerializeField] private TextMeshProUGUI _textDisplay;
	[SerializeField] private BubbleManager _bubbleManager;
	#endregion

	#region Fields
	private string _guessSentence = "";

	private List<char> _guessedCharacters = new List<char>();
	#endregion

	#region Properties
	public string GuessSentence => _guessSentence.ToLower();
	#endregion

	#region Events
	#endregion

	#region Mono
	private void OnEnable()
	{
		_bubbleManager.PressSuccess += OnKeySuccess;
		_bubbleManager.PressFailed += OnKeyFailed;

		GetSentenceBeginning();
		_bubbleManager.GenerateBubbles(GuessSentence);
	}
	private void OnDisable()
	{
		_bubbleManager.PressSuccess -= OnKeySuccess;
		_bubbleManager.PressFailed -= OnKeyFailed;
	}

	private void OnKeyFailed(object sender, EventArgs e)
	{
		GetSentenceBeginning();
		_guessedCharacters.Clear();

		_bubbleManager.ClearBubbles(this, EventArgs.Empty);
		_bubbleManager.GenerateBubbles(GuessSentence);
	}

	private void OnKeySuccess(object sender, string e)
	{
		// add the character to the guessed characters
		_guessedCharacters.Add(e.ToCharArray()[0]);

		UpdateText();
	}
	#endregion

	#region Methods
	public string GetSentenceBeginning()
	{
		_guessSentence = _sentenceData.GetRandomBeginning();

		SetTextEmpty(_guessSentence);
		return _guessSentence;
	}

	private void SetTextEmpty(string input)
	{
		string empty = "";

		foreach (char c in input)
		{
			if(c == ' ' ||  c == '\n' || c == '.')
			{
				empty += c;
				continue;
			}

			empty += '_';
		}

		_textDisplay.text = empty;
	}
	private void UpdateText()
	{
		string updated = "";

		foreach (char c in _guessSentence.ToLower())
		{
			if (c == ' ' || c == '\n' || c == '.' || _guessedCharacters.Contains(c))
			{
				updated += c;
				continue;
			}

			updated += '_';
		}

		_textDisplay.text = updated;
	}
	#endregion

	#region EventHandlers
	#endregion
}
