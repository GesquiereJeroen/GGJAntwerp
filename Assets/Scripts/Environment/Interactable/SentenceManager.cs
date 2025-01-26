using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SentenceManager : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private SentenceData _sentenceData;
	[SerializeField] private TextMeshProUGUI _textDisplay;
	[SerializeField] private BubbleManager _bubbleManager;

	[SerializeField] private float _timeBeforeReshuffle = 2f;
	[SerializeField] private float _typeTimeBetweenCharacters = 0.1f;

	[SerializeField] private RectTransform _speechBubblePivot;
	[SerializeField] private float _speechBubbleGrowTime;

	[SerializeField] private Image _speechBubbleImage;
	[SerializeField] private float _shakeDuration;
	#endregion

	#region Fields
	private string _guessSentence = "";

	private List<char> _neededCharacters = new List<char>();
	private List<char> _guessedCharacters = new List<char>();

	private int _currentSentencePart = 1;

	private bool _hasMadeError = false;
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

		_speechBubblePivot.DOScale(0, 0);

		GameManager.Instance.GameStarted += StartSentence;
	}

	private void OnDisable()
	{
		_bubbleManager.PressSuccess -= OnKeySuccess;
		_bubbleManager.PressFailed -= OnKeyFailed;

		_guessedCharacters.Clear();
		GameManager.Instance.GameStarted -= StartSentence;
	}
	#endregion

	#region Methods
	private void StartSentence(object sender, EventArgs e)
	{
		GetCurrentSentencePart();
	}
	private void OnKeyFailed(object sender, EventArgs e)
	{
		// always clear the bubbles on a wrong input
		_bubbleManager.ClearBubbles(this, EventArgs.Empty);
		_bubbleManager.CanSpawn = false;
		_bubbleManager.DetectInput = false;

		_speechBubbleImage.color = Color.red;
		_speechBubbleImage.transform.DOShakePosition(_shakeDuration, strength: 10, vibrato: 20, randomness: 100, fadeOut: false).OnComplete(() =>
		{
			_speechBubbleImage.color = Color.white;

			_bubbleManager.CanSpawn = true;
			_bubbleManager.DetectInput = true;

			// give the player 1 extra chance
			if (!_hasMadeError)
			{
				_hasMadeError = true;
				return;
			}

			_guessedCharacters.Clear();

			GameManager.Instance.LoseLife();

			if (GameManager.Instance.CurrentHealth <= 0)
			{
				_speechBubblePivot.DOScale(0, _speechBubbleGrowTime)
					.SetEase(Ease.OutQuad);
				return;
			}

			// get a new sentence part
			GetCurrentSentencePart();
		});
	}

	private void OnKeySuccess(object sender, string e)
	{
		char typedCharacter = e.ToCharArray()[0];

		// add the character to the guessed characters
		// assume the string provided is just the character
		_guessedCharacters.Add(typedCharacter);

		_bubbleManager.NeededCharacters.Remove(typedCharacter);

		UpdateText();

		CheckSentenceComplete();
	}

	private void CheckSentenceComplete()
	{
		foreach (char needed in _neededCharacters)
		{
			// if character is not guessed then return and stop doing checks
			if (!_guessedCharacters.Contains(needed))
			{
				return;
			}
		}

		// sentence is complete so proceed to the next sentence
		++_currentSentencePart;

		_bubbleManager.DetectInput = false;

		// 3 sentences have been solved
		if (_currentSentencePart > 3)
		{
			DOVirtual.DelayedCall(_timeBeforeReshuffle, () =>
			{
				_speechBubblePivot.DOScale(0, _speechBubbleGrowTime)
					.OnComplete(GameManager.Instance.WinGame)
					.SetEase(Ease.OutQuad);
			});

			return;
		}

		DOVirtual.DelayedCall(_timeBeforeReshuffle, () =>
		{
			_speechBubblePivot.DOScale(0, _speechBubbleGrowTime)
				.OnComplete(GetCurrentSentencePart)
				.SetEase(Ease.OutQuad);
		});
	}

	private void GetCurrentSentencePart()
	{
		switch (_currentSentencePart)
		{
			case 1:
				GetSentenceBeginning();
				break;
			case 2:
				GetSentenceMiddle();
				break;
			case 3:
				GetSentenceEnding();
				break;
		}

		_guessSentence = _guessSentence.ToUpper();

		_hasMadeError = false;

		_guessedCharacters.Clear();
		_neededCharacters.Clear();
		_bubbleManager.NeededCharacters.Clear();
		UpdateText();

		_bubbleManager.CanSpawn = false;

		_textDisplay.maxVisibleCharacters = 0;

		// grow the speech bubble
		// then type the new empty text
		_speechBubblePivot.DOScale(1, _speechBubbleGrowTime)
			.OnComplete(() => TypeEmptyText(() =>
			{
				_bubbleManager.CanSpawn = true;
				_bubbleManager.DetectInput = true;
			}))
			.SetEase(Ease.OutBack);

		GetNeededCharacters();
	}

	private void GetNeededCharacters()
	{
		foreach (var c in GuessSentence)
		{
			if (IsCharacterAllowed(c)) continue;

			if (!_neededCharacters.Contains(c))
			{
				_neededCharacters.Add(c);
				_bubbleManager.NeededCharacters.Add(c);
			}
		}
	}

	public string GetSentenceBeginning()
	{
		_guessSentence = _sentenceData.GetRandomBeginning();
		GameManager.Instance.BeginningText = _guessSentence;

		return _guessSentence;
	}
	public string GetSentenceMiddle()
	{
		_guessSentence = _sentenceData.GetRandomMiddle();
		GameManager.Instance.MiddleText = _guessSentence;

		return _guessSentence;
	}
	public string GetSentenceEnding()
	{
		_guessSentence = _sentenceData.GetRandomEnding();
		GameManager.Instance.EndingText = _guessSentence;

		return _guessSentence;
	}
	private void UpdateText()
	{
		string updated = "";

		foreach (char c in _guessSentence.ToLower())
		{
			if (IsCharacterAllowed(c) || _guessedCharacters.Contains(c))
			{
				updated += c.ToString().ToUpper();
				continue;
			}

			updated += '_';
		}

		_textDisplay.text = updated;
	}

	private void TypeEmptyText(Action onComplete = null)
	{
		// get the amount of character that we have loop over
		int charAmount = _textDisplay.text.Length;

		// tween to start the characters
		DOVirtual.Int(0, charAmount, charAmount * _typeTimeBetweenCharacters, AddLetterToText)
			.OnComplete(() => onComplete?.Invoke())
			.SetEase(Ease.Linear)
			.SetId(transform);
	}
	private void AddLetterToText(int value)
	{
		// set the max visible characters to current tween value
		_textDisplay.maxVisibleCharacters = value;
	}

	private static bool IsCharacterAllowed(char c)
	{
		return c == ' ' || c == '\n' || c == '.' || c == ',';
	}
	#endregion

	#region EventHandlers
	#endregion
}
