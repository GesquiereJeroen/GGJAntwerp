using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubbleScriptForMenu : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private TextMeshProUGUI _letterDisplay;
    #endregion

    #region Fields
    private KeyCode _key;
    #endregion

    #region Properties
    public KeyCode KeyToPress
    {
        get => _key;
        set
        {
            _key = value;
            UpdateText();
        }
    }


    public MenuBubble BubbleManager { get; set; }
    #endregion

    #region Events
    public event EventHandler Destroyed;
    #endregion

    #region Mono
    private void Awake()
    {

    }
    private void OnDestroy()
    {
        var handler = Destroyed;
        handler?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Methods
    public void Pop()
    {
        if (gameObject == null) return;
        Destroy(gameObject);
    }

    private void UpdateText()
    {
        if (_key == KeyCode.None)
        {
            _letterDisplay.text = "";
            return;
        }
        _letterDisplay.text = _key.ToString().ToUpper();
    }
    #endregion

    #region EventHandlers
    #endregion
}
