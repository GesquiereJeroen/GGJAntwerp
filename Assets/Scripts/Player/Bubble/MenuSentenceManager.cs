using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSentenceManager : MonoBehaviour
{
    #region Editor Fields
    [SerializeField] private SentenceData _sentenceData;
    [SerializeField] private TextMeshProUGUI _textDisplay;
    [SerializeField] private MenuBubble _bubbleManager;

    [SerializeField] private float _timeBeforeReshuffle = 2f;
    #endregion

    #region Fields
    private string _guessSentence = "";

    private List<char> _neededCharacters = new List<char>();
    private List<char> _guessedCharacters = new List<char>();

    private int _currentSentencePart = 1;
    #endregion

    #region Properties
    public string GuessSentence => _guessSentence.ToLower();
    #endregion

    #region Events
    #endregion

    #region Mono
    private void OnEnable()
    {
        _bubbleManager.PressSuccess += OnKeySuccess;
        _bubbleManager.PressFailed += OnKeyFailed;

        GetCurrentSentencePart();
    }
    private void OnDisable()
    {
        _bubbleManager.PressSuccess -= OnKeySuccess;
        _bubbleManager.PressFailed -= OnKeyFailed;

        _guessedCharacters.Clear();
    }
    #endregion

    #region Methods
    private void OnKeyFailed(object sender, EventArgs e)
    {
        //_guessedCharacters.Clear();
        //GetCurrentSentencePart();

        //_bubbleManager.ClearBubbles(this, EventArgs.Empty);

        //GameManager.Instance.LoseLife();
    }
    [SerializeField] TextMeshProUGUI greyedText;
    private void OnKeySuccess(object sender, string e)
    {
        char typedCharacter = e.ToCharArray()[0];

        // add the character to the guessed characters
        // assume the string provided is just the character
        _guessedCharacters.Add(typedCharacter);

        _bubbleManager.NeededCharacters.Remove(typedCharacter);

        UpdateText();
        greyedText.GetComponent<VertexWobble>().RemoveCharacter(typedCharacter);
        CheckSentenceComplete();
    }

    private void CheckSentenceComplete()
    {
        foreach (char needed in _neededCharacters)
        {
            // if character is not guessed then return and stop doing checks
            if (!_guessedCharacters.Contains(needed))
            {
                return;
            }
        }

        Invoke("ChangeScene",3);
    }


    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }


    private void GetCurrentSentencePart()
    {
        switch (_currentSentencePart)
        {
            case 1:
                GetSentenceBeginning();
                break;
            case 2:
                GetSentenceMiddle();
                break;
            case 3:
                GetSentenceEnding();
                break;
        }

        _guessSentence = _guessSentence.ToUpper();

        _guessedCharacters.Clear();
        _neededCharacters.Clear();
        UpdateText();
        GetNeededCharacters();
    }

    private void GetNeededCharacters()
    {
        foreach (var c in GuessSentence)
        {
            if (IsCharacterAllowed(c)) continue;

            if (!_neededCharacters.Contains(c))
            {
                _neededCharacters.Add(c);
                _bubbleManager.NeededCharacters.Add(c);
            }
        }
    }

    public string GetSentenceBeginning()
    {
        _guessSentence = _sentenceData.GetRandomBeginning();
       // GameManager.Instance.BeginningText = _guessSentence;

        return _guessSentence;
    }
    public string GetSentenceMiddle()
    {
        _guessSentence = _sentenceData.GetRandomMiddle();
        GameManager.Instance.MiddleText = _guessSentence;

        return _guessSentence;
    }
    public string GetSentenceEnding()
    {
        _guessSentence = _sentenceData.GetRandomEnding();
        GameManager.Instance.EndingText = _guessSentence;

        return _guessSentence;
    }
    private void UpdateText()
    {
        string updated = "";

        foreach (char c in _guessSentence.ToLower())
        {
            if (IsCharacterAllowed(c) || _guessedCharacters.Contains(c))
            {
                updated += c;
                continue;
            }

            updated += '_';
        }

        _textDisplay.text = updated;
    }

    private static bool IsCharacterAllowed(char c)
    {
        return c == ' ' || c == '\n' || c == '.' || c == ',';
    }
    #endregion

    #region EventHandlers
    #endregion
}
