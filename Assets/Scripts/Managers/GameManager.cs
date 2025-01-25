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

	[Header("EndGame")]
	[SerializeField] private float _ribbonMoveTime;
	[SerializeField] private RectTransform _ribbonTransform;
	[SerializeField] private TextMeshProUGUI _ribbonText;

	[SerializeField] private List<string> _winMessages;
	[SerializeField] private List<string> _loseMessages;

	[SerializeField] private TextMeshProUGUI _returnInstruction;
	#endregion

	#region Fields
	private int _currentHealth;

	private bool _canReturnToMenu = false;
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

		_returnInstruction.maxVisibleCharacters = 0;
		_returnInstruction.gameObject.SetActive(false);


		foreach (var healthPoint in _healthPoints)
		{
			healthPoint.SetActive(true);
		}

		GameEnded += SlideRibbon;
	}


	private void Start()
	{
		OnGameStarted();
	}
	private void Update()
	{
		if (!_canReturnToMenu) return;

		if(Input.GetKeyDown(KeyCode.Space)) 
		{
			// load the main menu;
			SceneManager.LoadScene(0);
		}
	}
	#endregion

	#region Methods

	private void SlideRibbon(object sender, bool hasWon)
	{
		if (hasWon)
		{
			var index = UnityEngine.Random.Range(0, _winMessages.Count);
			_ribbonText.text = _winMessages[index];
		}
		else
		{
			var index = UnityEngine.Random.Range(0, _loseMessages.Count);
			_ribbonText.text = _loseMessages[index];

			_ribbonTransform.DOLocalMoveX(0, _ribbonMoveTime)
				.SetEase(Ease.OutBack)
				.OnComplete(() =>
				{
					_returnInstruction.gameObject.SetActive(true);

					TypeText(_returnInstruction);
					_canReturnToMenu = true;
				});
		}
	}
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

		_winSpeechBubbleText.text = $"{BeginningText} {MiddleText} {EndingText}".ToUpper();
		_winSpeechbubble.gameObject.SetActive(true);

		_winSpeechbubble.DOScale(1, _scaleTime)
			.OnComplete(() => TypeText(_winSpeechBubbleText, () => 
			{
				_ribbonTransform.DOLocalMoveX(0, _ribbonMoveTime)
					.SetEase(Ease.OutBack)
					.OnComplete(() => 
					{
						_returnInstruction.gameObject.SetActive(true);

						TypeText(_returnInstruction); 
						_canReturnToMenu = true; 
					});
			}));
	}
	private void TypeText(TextMeshProUGUI text, Action onComplete = null)
	{
		// get the amount of character that we have loop over
		int charAmount = text.text.Length;

		// tween to start the characters
		DOVirtual.Int(0, charAmount, charAmount * _typeTimeBetweenCharacters, (v) => AddLetterToText(v, text))
			.OnComplete(() => onComplete?.Invoke())
			.SetEase(Ease.Linear)
			.SetId(transform);
	}
	private void AddLetterToText(int value, TextMeshProUGUI text)
	{
		// set the max visible characters to current tween value
		text.maxVisibleCharacters = value;
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
