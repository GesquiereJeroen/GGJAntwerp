using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleManager : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private Bubble _bubblePrefab;
	[SerializeField] private Vector2 _spawnInterval = new Vector2(0.2f, 1f);
	[SerializeField] private float _emptyChance;
	#endregion

	#region Fields
	private List<Bubble> _bubbles = new List<Bubble>();
	private List<char> _neededCaharacters = new List<char>();
	private readonly Dictionary<char, KeyCode> _keycodeCache = new Dictionary<char, KeyCode>();

	private bool _correctKeyPressed;
	private bool _doOnce = true;

	private List<KeyCode> _alwaysLegalCodes = new List<KeyCode>()
	{
		KeyCode.Space,
		KeyCode.LeftAlt,
		KeyCode.RightAlt,
		KeyCode.Mouse0,
		KeyCode.Mouse1,
		KeyCode.Mouse2,
		KeyCode.Mouse3,
		KeyCode.Mouse4,
		KeyCode.LeftControl,
		KeyCode.RightControl,
		KeyCode.LeftShift,
		KeyCode.RightShift,
		KeyCode.CapsLock,
		KeyCode.Escape,
		KeyCode.Tab,

	};

	private float _timer;
	private float _currentInterval;

	private bool _gameEnded;
	#endregion

	#region Properties
	public List<char> NeededCharacters => _neededCaharacters;
	#endregion

	#region Events
	public event EventHandler<string> PressSuccess;
	public event EventHandler PressFailed;
	#endregion

	#region Mono
	private void Awake()
	{
		_currentInterval = Random.Range(_spawnInterval.x, _spawnInterval.y);
	}
	private void OnEnable()
	{
		GameManager.Instance.GameEnded += SetGameEnded;
	}

	private void OnDisable()
	{
		GameManager.Instance.GameEnded -= SetGameEnded;
	}
	private void FixedUpdate()
	{
		if (_gameEnded) return;
		_timer += Time.deltaTime;
		if(_timer >= _currentInterval)
		{
			SpawnBubble();
			_currentInterval = Random.Range(_spawnInterval.x, _spawnInterval.y);
			_timer = 0;
		}
	}
	private void Update()
	{
		_correctKeyPressed = false;
		_doOnce = true;

		// don't check for input of there are no bubbles
		if (_bubbles.Count <= 0) return;

		// if no input then don't check for the specific keys
		if (!Input.anyKeyDown) return;

		foreach (KeyCode key in _alwaysLegalCodes)
		{
			if (Input.GetKeyDown(key))
				return;
		}

		// go over all the bubbles to see if the any of the correct keys have been pressed
		for (int i = _bubbles.Count -1; i >= 0; i--)
		{
			var currentBubble = _bubbles[i];
			// check if any of the keys pressed correspond with any of the bubbles currently on screen
			if (Input.GetKeyDown(currentBubble.KeyToPress))
			{
				currentBubble.Pop();
				if (!_correctKeyPressed)
				{
					// only execute this once
					OnPressSuccess(currentBubble.KeyToPress);
					_correctKeyPressed = true;
				}
			}
		}

		// if the correct key was pressed then stop doing checks
		if (_correctKeyPressed) return;

		if (_doOnce)
		{
			_doOnce = false;
			OnPressFailed();
		}

	}
	#endregion

	#region Methods
	private void SpawnBubble()
	{
		if(_neededCaharacters.Count <= 0) return;

		var bubble = Instantiate(_bubblePrefab);

		char randomCharacterNeeded = _neededCaharacters[Random.Range(0, _neededCaharacters.Count)];

		if(Random.Range(0f, 1f) > _emptyChance)
		{
			bubble.KeyToPress = GetKeyCode(randomCharacterNeeded);
		}
		else
		{
			bubble.KeyToPress = KeyCode.None;
		}

		bubble.BubbleManager = this;

		bubble.Destroyed += OnBubbleDestroyed;

		bubble.transform.position = transform.position + (Vector3)Random.insideUnitCircle;

		_bubbles.Add(bubble);
	}

	private KeyCode GetKeyCode(char character)
	{
		// Get from cache if it was taken before to prevent unnecessary enum parse
		KeyCode code;
		if (_keycodeCache.TryGetValue(character, out code)) return code;

		// Cast to its integer value
		int alphaValue = character;
		code = (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString());
		_keycodeCache.Add(character, code);
		return code;
	}

	private void OnBubbleDestroyed(object sender, EventArgs e)
	{
		var bubble = sender as Bubble;

		_bubbles.Remove(bubble);
		bubble.Destroyed -= OnBubbleDestroyed;
	}
	public void ClearBubbles(object sender, EventArgs e)
	{
		for (int i = _bubbles.Count - 1; i >= 0; i--)
		{
			Destroy(_bubbles[i].gameObject);
		}
	}

	private void SetGameEnded(object sender, bool e)
	{
		_gameEnded = true;
	}

	#endregion

	#region EventHandlers
	private void OnPressFailed()
	{
		var handler = PressFailed;
		handler?.Invoke(this, EventArgs.Empty);
	}
	private void OnPressSuccess(KeyCode key)
	{
		var handler = PressSuccess;
		handler?.Invoke(this, key.ToString().ToLower());
	}
	#endregion
}
