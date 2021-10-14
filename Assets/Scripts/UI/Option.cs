using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OptionType
{
    Retry,
    Load,
    Setting,
    Title
}

public class Option : MonoBehaviour
{
    OptionType _preType = OptionType.Retry;
    int _preIndex = 0;
    List<OptionType> _optionList;

    [SerializeField] Image[] _optionButton;
    [SerializeField] Color _highlightedColor;
    [SerializeField] Text[] _optionButtonText;
    [SerializeField] int _defaultFontSize;
    [SerializeField] int _selectedFontSize;

    AudioSource audioSource;

    bool _onUp;
    bool _onDown;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        _optionList = new List<OptionType>();
        _optionList.Add(OptionType.Retry);
        _optionList.Add(OptionType.Load);
        _optionList.Add(OptionType.Setting);
        _optionList.Add(OptionType.Title);
    }

    private void Update()
    {
        GetInput();
        Select();
    }

    private void OnEnable()
    {
        CheckOption();
    }

    void GetInput()
    {
        _onUp = Input.GetButtonDown("Up");
        _onDown = Input.GetButtonDown("Down");

        if (_onUp && !_onDown || !_onUp && _onDown)
            CheckDirection(_onUp);
    }

    void CheckDirection(bool up)
    {
        audioSource.Play();

        if (up)
        {
            _preIndex--;

            if (_preIndex < 0)
                _preIndex = _optionList.Count - 1;

            _preType = _optionList[_preIndex];
        }
        else
        {
            _preIndex++;

            if (_preIndex >= _optionList.Count)
                _preIndex = 0;

            _preType = _optionList[_preIndex];
        }

        CheckOption();
    }

    void CheckOption()
    {
        for (int i = 0; i < _optionButton.Length; i++)
        {
            _optionButton[i].color = new Color(1, 1, 1, 1);
            _optionButtonText[i].fontSize = _defaultFontSize;
        }

        _optionButton[_preIndex].color = _highlightedColor;
        _optionButtonText[_preIndex].fontSize = _selectedFontSize;
    }

    void Select()
    {
        if (Input.GetButtonDown("Submit"))
        {
            switch (_preType)
            {
                case OptionType.Retry:
                    GameManager.instance.StageManagerInstance.Retry();
                    break;
                case OptionType.Load:
                    break;
                case OptionType.Setting:
                    break;
                case OptionType.Title:
                    GameManager.instance.StageManagerInstance.Title();
                    break;
            }
        }
    }
}
