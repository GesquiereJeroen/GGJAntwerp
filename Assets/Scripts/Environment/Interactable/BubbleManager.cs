using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleManager : MonoBehaviour
{
	#region Editor Fields
	[SerializeField] private Bubble _bubblePrefab;
	#endregion

	#region Fields
	private List<Bubble> _bubbles = new List<Bubble>();
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
	#endregion

	#region Properties

	#endregion

	#region Events
	public event EventHandler<string> PressSuccess;
	public event EventHandler PressFailed;
	#endregion

	#region Mono
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
		foreach (var bubble in _bubbles)
		{
			// check if any of the keys pressed correspond with any of the bubbles currently on screen
			if (Input.GetKeyDown(bubble.KeyToPress))
			{
				bubble.Pop();
				if (!_correctKeyPressed)
				{
					// only execute this once
					OnPressSuccess(bubble.KeyToPress);
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
	public void GenerateBubbles(string guessSentence)
	{
		foreach (char c in guessSentence)
		{
			if (c == ' ') continue;

			var bubble = Instantiate(_bubblePrefab);
			bubble.KeyToPress = GetKeyCode(c);
			bubble.BubbleManager = this;

			bubble.Destroyed += OnBubbleDestroyed;

			bubble.transform.position = Random.insideUnitCircle * 5;

			_bubbles.Add(bubble);
		}
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
