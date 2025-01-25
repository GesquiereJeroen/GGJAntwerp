using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region Editor Fields
	[Header("Health")]
	[SerializeField] private int _startHealth;
	[SerializeField] private GameObject[] _healthPoints;


	[Header("Text Displays")]
	[SerializeField] private float _typeTimeBetweenCharacters = 0.05f;
	[SerializeField] private RectTransform _winSpeechbubble;
	[SerializeField] private TextMeshProUGUI _winSpeechBubbleText;
	[SerializeField] private float _scaleTime;
	#endregion

	#region Fields
	private int _currentHealth;
	#endregion

	#region Properties
	public static GameManager Instance;

	public string BeginningText { get; set; }
	public string MiddleText { get; set; }
	public string EndingText { get; set; }

	#endregion

	#region Events
	public event EventHandler GameStarted;
	public event EventHandler<bool> GameEnded;
	#endregion

	#region Mono
	private void Awake()
	{
		Instance = this;
		_currentHealth = _startHealth;

		_winSpeechBubbleText.maxVisibleCharacters = 0;
		_winSpeechbubble.gameObject.SetActive(false);
		_winSpeechbubble.DOScale(0, 0);

		foreach (var healthPoint in _healthPoints)
		{
			healthPoint.SetActive(true);
		}
	}
	private void Start()
	{
		OnGameStarted();
	}
	#endregion

	#region Methods
	public void LoseLife()
	{
		--_currentHealth;

		UpdateHealth();

		if (_currentHealth <= 0)
		{
			LoseGame();
		}
	}

	private void UpdateHealth()
	{
		_healthPoints[_currentHealth].SetActive(false);
	}

	private void LoseGame()
	{
		OnGameEnded(false);
	}

	public void WinGame()
	{
		OnGameEnded(true);

		_winSpeechBubbleText.text = $"{BeginningText} {MiddleText} {EndingText}";
		_winSpeechbubble.gameObject.SetActive(true);

		_winSpeechbubble.DOScale(1, _scaleTime)
			.OnComplete(() => TypeText());
	}
	private void TypeText(Action onComplete = null)
	{
		// get the amount of character that we have loop over
		int charAmount = _winSpeechBubbleText.text.Length;

		// tween to start the characters
		DOVirtual.Int(0, charAmount, charAmount * _typeTimeBetweenCharacters, AddLetterToText)
			.OnComplete(() => onComplete?.Invoke())
			.SetEase(Ease.Linear)
			.SetId(transform);
	}
	private void AddLetterToText(int value)
	{
		// set the max visible characters to current tween value
		_winSpeechBubbleText.maxVisibleCharacters = value;
	}
	#endregion

	#region EventHandlers
	private void OnGameStarted()
	{
		var handler = GameStarted;
		handler?.Invoke(this, EventArgs.Empty);
	}
	private void OnGameEnded(bool hasWon)
	{
		var handler = GameEnded;
		handler?.Invoke(this, hasWon);
	}
	#endregion
}
