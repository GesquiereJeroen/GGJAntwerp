using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private AudioSource _source;
	#endregion

	#region Fields
	#endregion

	#region Properties
	public static MusicManager Instance;
	#endregion

	#region Events
	#endregion

	#region Mono
	private void Awake()
	{
		// prevents multiple musicmanagers from existing
		if (Instance != null && Instance != this)
		{
			Destroy(this);

			return;
		}
		else
		{
			//If not created before set the instance to the current script
			Instance = this;
		}

		DontDestroyOnLoad(gameObject);

		_source.Play();
	}
	#endregion

	#region Methods
	#endregion

	#region EventHandlers
	#endregion
}
