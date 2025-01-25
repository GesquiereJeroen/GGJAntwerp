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
	[SerializeField] private int _startHealth;

	[Header("Text Displays")]
	[SerializeField] private float _appearDelay;
	[SerializeField] private TextMeshProUGUI _beginningText;
	[SerializeField] private TextMeshProUGUI _middleText;
	[SerializeField] private TextMeshProUGUI _endingText;
	#endregion

	#region Fields
	private int _currentHealth;
    #endregion

    #region Properties
	public static GameManager Instance;

	public string BeginningText { get; set; }
	public string MiddleText {  get; set; }
	public string EndingText { get; set; }

	#endregion

	#region Events
	public event EventHandler<bool> GameEnded;
	#endregion

	#region Mono
	private void Awake()
	{
		Instance = this;
		_currentHealth = _startHealth;

		_beginningText.text = "";
		_middleText.text = "";
		_endingText.text = "";
	}
	#endregion

	#region Methods
	public void LoseLife()
	{
		--_currentHealth;

		if(_currentHealth <= 0)
		{
			LoseGame();
		}

	}

	private void LoseGame()
	{
		OnGameEnded(false);
	}

	public void WinGame()
	{
		OnGameEnded(true);

		DOVirtual.DelayedCall(_appearDelay, () =>
		{
			_beginningText.text = BeginningText;
			_beginningText.gameObject.SetActive(true);
			DOVirtual.DelayedCall(_appearDelay, () =>
			{
				_middleText.text = MiddleText;
				_middleText.gameObject.SetActive(true);
				DOVirtual.DelayedCall(_appearDelay, () =>
				{
					_endingText.text = EndingText;
					_endingText.gameObject.SetActive(true);
				});
			});
		});
	}
	#endregion

	#region EventHandlers
	private void OnGameEnded(bool hasWon)
	{
		var handler = GameEnded;
		handler?.Invoke(this, hasWon);
	}
	#endregion
}
