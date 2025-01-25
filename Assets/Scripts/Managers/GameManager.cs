using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private int _startHealth;
	#endregion

	#region Fields
	private int _currentHealth;
    #endregion

    #region Properties
	public static GameManager Instance;
	#endregion

	#region Events
	#endregion

	#region Mono
	private void Awake()
	{
		Instance = this;
		_currentHealth = _startHealth;
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
		
	}

	public void WinGame()
	{
		Debug.Log("Yippeeee");
	}
	#endregion

	#region EventHandlers
	#endregion
}
