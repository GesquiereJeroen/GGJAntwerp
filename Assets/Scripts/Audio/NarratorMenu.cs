using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorMenu : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private Vector2 _randomInterval;
	[SerializeField] private AudioSource _source;
	[SerializeField] private AudioClip _beginning;
	[SerializeField] private AudioClip _prompt;
	#endregion

	#region Fields
	private float _timer;
	private float _countDown;

	private bool _canPrompt;
	#endregion

	#region Properties
	#endregion

	#region Events
	#endregion

	#region Mono
	private void Awake()
	{
		_countDown = Random.Range(_randomInterval.x, _randomInterval.y);

		PlayClip(_beginning);
		DOVirtual.DelayedCall(_beginning.length, () => _canPrompt = true);
	}

	private void PlayClip(AudioClip clip)
	{
		_source.clip = clip;
		_source.Play();
	}

	private void Update()
	{
		if (!_canPrompt) return;

		_timer += Time.deltaTime;
		if(_timer >= _countDown)
		{
			_timer = 0;
			_countDown = Random.Range(_randomInterval.x, _randomInterval.y);
			PlayClip(_prompt);
		}
	}
	#endregion

	#region Methods
	#endregion

	#region EventHandlers
	#endregion
}
