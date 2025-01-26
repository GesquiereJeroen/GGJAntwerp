//using System;
//using System.Collections.Generic;
//using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.DualShock;
//using UnityEngine.InputSystem.Switch;
//using UnityEngine.InputSystem.XInput;

//public class InputManager
//{
//	#region Enums
//	/// <summary>
//	/// Describe the different Input Actions from the Input Action Asset 'GameInput' into the enums 'Player' and 'UI'
//	/// then assign them in the 'AssignInputActions' method in the same order as the enum
//	/// </summary>

//	public enum Player
//	{
//		Move,
//		Jump,
//	}
//	public enum UI
//	{
//		Navigate,
//		Submit,
//		Cancel,
//		Point,
//		Click,
//		ScrollWheel,
//		MiddleClick,
//		RightClick,
//		TrackedDevicePosition,
//		TrackedDeviceRotation,
//	}

//	public enum InputType
//	{
//		Keyboard,
//		Playstation,
//		Xbox,
//		Switch,
//		GenericGamePad
//	}
//	#endregion

//	#region Fields
//	private List<InputAction> _playerInputActions = new List<InputAction>();
//	private List<InputAction> _UIInputActions = new List<InputAction>();
//	private GameInput _inputSystem;

//	private bool _lastInputWasController = false;
//	private InputType _currentDeviceType;
//	#endregion

//	#region Properties
//	public InputType CurrentDeviceType
//	{
//		get
//		{
//			return _currentDeviceType;
//		}
//		set
//		{
//			if (_currentDeviceType != value)
//			{
//				_currentDeviceType = value;
//				OnDeviceSwitch(_currentDeviceType);
//			}
//		}
//	}

//	public event EventHandler<DeviceSwitchEventArgs> DeviceSwitch;

//	private static InputManager _instance;
//	public static InputManager Instance
//	{
//		get
//		{
//			// compund assignment, checks to see if it exists if it doesn't it creates one
//			_instance ??= new InputManager();
//			return _instance;
//		}
//	}
//	#endregion

//	private InputManager()
//	{
//		_inputSystem = new GameInput();

//		AssignInputActions();

//		EnableAll();
//		SetupDeviceType();
//	}

//	#region Methods
//	private void AssignInputActions()
//	{
//		// assign these in the same order as the enums above

//		// Player Map
//		_playerInputActions.Add(_inputSystem.Player.Move);
//		_playerInputActions.Add(_inputSystem.Player.Jump);
//		// UI Map
//		_UIInputActions.Add(_inputSystem.UI.Navigate);
//		_UIInputActions.Add(_inputSystem.UI.Submit);
//		_UIInputActions.Add(_inputSystem.UI.Cancel);
//		_UIInputActions.Add(_inputSystem.UI.Point);
//		_UIInputActions.Add(_inputSystem.UI.Click);
//		_UIInputActions.Add(_inputSystem.UI.ScrollWheel);
//		_UIInputActions.Add(_inputSystem.UI.MiddleClick);
//		_UIInputActions.Add(_inputSystem.UI.Rightclick);
//		_UIInputActions.Add(_inputSystem.UI.TrackedDevicePostion);
//		_UIInputActions.Add(_inputSystem.UI.TrackedDeviceOrientation);
//	}

//	#region Internal
//	private InputAction GetAction(Player inputType)
//	{
//		return _playerInputActions[(int)inputType];
//	}

//	private void EnableAction(Player input)
//	{
//		_playerInputActions[(int)input].Enable();
//	}
//	private void EnableAction(UI input)
//	{
//		_UIInputActions[(int)input].Enable();
//	}

//	private void DisableAction(Player input)
//	{
//		_playerInputActions[(int)input].Disable();
//	}
//	private void DisableAction(UI input)
//	{
//		_UIInputActions[(int)input].Disable();
//	}
//	public void EnableAllPlayer()
//	{
//		foreach (var action in _playerInputActions)
//		{
//			action.Enable();
//		}
//	}
//	public void EnableAllUI()
//	{
//		foreach (var action in _UIInputActions)
//		{
//			action.Enable();
//		}
//	}

//	public void DisableAllPlayer()
//	{
//		foreach (var action in _playerInputActions)
//		{
//			action.Disable();
//		}
//	}
//	public void DisableAllUI()
//	{
//		foreach (var action in _UIInputActions)
//		{
//			action.Disable();
//		}
//	}

//	public void EnableAll()
//	{
//		EnableAllPlayer();
//		EnableAllUI();
//	}
//	public void DisableAll()
//	{
//		DisableAllPlayer();
//		DisableAllUI();
//	}
//	#endregion

//	#region Events

//	public void SubscribeOnPerform(Player input, Action<InputAction.CallbackContext> function)
//	{
//		_playerInputActions[(int)input].performed += function;
//	}
//	public void SubscribeOnPerform(UI input, Action<InputAction.CallbackContext> function)
//	{
//		_UIInputActions[(int)input].performed += function;
//	}

//	public void UnsubscribeOnPerform(Player input, Action<InputAction.CallbackContext> function)
//	{
//		_playerInputActions[(int)input].performed -= function;
//	}
//	public void UnsubscribeOnPerform(UI input, Action<InputAction.CallbackContext> function)
//	{
//		_UIInputActions[(int)input].performed -= function;
//	}

//	public void SubscribeOnCancel(Player input, Action<InputAction.CallbackContext> function)
//	{
//		_playerInputActions[(int)input].canceled += function;
//	}
//	public void SubscribeOnCancel(UI input, Action<InputAction.CallbackContext> function)
//	{
//		_UIInputActions[(int)input].canceled += function;
//	}

//	public void UnsubscribeOnCancel(Player input, Action<InputAction.CallbackContext> function)
//	{
//		_playerInputActions[(int)input].canceled -= function;
//	}
//	public void UnsubscribeOnCancel(UI input, Action<InputAction.CallbackContext> function)
//	{
//		_UIInputActions[(int)input].canceled -= function;
//	}
//	#endregion

//	#region Static Methods
//	public static InputAction GetInputAction(Player inputType)
//	{
//		return Instance.GetAction(inputType);
//	}

//	public static void EnableInputAction(Player input)
//	{
//		Instance.EnableAction(input);
//	}
//	public static void EnableInputAction(UI input)
//	{
//		Instance.EnableAction(input);
//	}

//	public static void DisableInputAction(Player input)
//	{
//		Instance.DisableAction(input);
//	}
//	public static void DisableInputAction(UI input)
//	{
//		Instance.DisableAction(input);
//	}
//	public static void EnableAllPlayerActions()
//	{
//		Instance.EnableAllPlayer();
//	}
//	public static void EnableAllUIActions()
//	{
//		Instance.EnableAllUI();
//	}

//	public static void DisableAllPlayerActions()
//	{
//		Instance.DisableAllPlayer();
//	}
//	public static void DisableAllUIActions()
//	{
//		Instance.DisableAllUI();
//	}

//	public static void EnableAllActions()
//	{
//		Instance.EnableAll();
//	}
//	public static void DisableAllActions()
//	{
//		Instance.DisableAll();
//	}

//	public static void SubscribeToPerformed(Player input, Action<InputAction.CallbackContext> function)
//	{
//		Instance.SubscribeOnPerform(input, function);
//	}
//	public static void SubscribeToPerformed(UI input, Action<InputAction.CallbackContext> function)
//	{
//		Instance.SubscribeOnPerform(input, function);
//	}

//	public static void UnsubscribeFromPerformed(Player input, Action<InputAction.CallbackContext> function)
//	{
//		Instance.UnsubscribeOnPerform(input, function);
//	}
//	public static void UnsubscribeFromPerformed(UI input, Action<InputAction.CallbackContext> function)
//	{
//		Instance.UnsubscribeOnPerform(input, function);
//	}

//	public static void SubscribeToCancelled(Player input, Action<InputAction.CallbackContext> function)
//	{
//		Instance.SubscribeOnCancel(input, function);
//	}
//	public static void SubscribeToCancelled(UI input, Action<InputAction.CallbackContext> function)
//	{
//		Instance.SubscribeOnCancel(input, function);
//	}

//	public static void UnsubscribeFromCancelled(Player input, Action<InputAction.CallbackContext> function)
//	{
//		Instance.UnsubscribeOnCancel(input, function);
//	}
//	public static void UnsubscribeFromCancelled(UI input, Action<InputAction.CallbackContext> function)
//	{
//		Instance.UnsubscribeOnCancel(input, function);
//	}
//	#endregion
//	#endregion

//	#region Device Switch
//	private void SetupDeviceType()
//	{
//		//Subscribe to all the inputaction.started to know what the last input was
//		foreach (var action in _playerInputActions)
//		{
//			action.started += ListenToDeviceType;
//		}
//	}

//	private void ListenToDeviceType(InputAction.CallbackContext context)
//	{
//		//Check the context to see if the input was from a controller or keyboard
//		_lastInputWasController = context.control.device is Gamepad or Joystick;

//		if (!_lastInputWasController)
//		{
//			CurrentDeviceType = InputType.Keyboard;
//			return;
//		}

//		var currentGamepad = Gamepad.current;
//		if (currentGamepad is DualShockGamepad)
//		{
//			CurrentDeviceType = InputType.Playstation;
//		}
//		else if (currentGamepad is XInputController)
//		{
//			CurrentDeviceType = InputType.Xbox;
//		}
//		else if (currentGamepad is SwitchProControllerHID)
//		{
//			CurrentDeviceType = InputType.Switch;
//		}
//		else
//		{
//			CurrentDeviceType = InputType.GenericGamePad;
//		}
//	}

//	private void OnDeviceSwitch(InputType type)
//	{
//		var handler = DeviceSwitch;
//		handler?.Invoke(this, new DeviceSwitchEventArgs(type));
//	}
//}
//public class DeviceSwitchEventArgs : EventArgs
//{
//	public InputManager.InputType Type { get; private set; }

//	public DeviceSwitchEventArgs(InputManager.InputType type)
//	{
//		Type = type;
//	}
//}
//#endregion
