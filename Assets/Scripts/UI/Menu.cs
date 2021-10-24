using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum menuType
{
    NewGame,
    Continue,
    Option,
    Exit
}

public class Menu : MonoBehaviour
{
    menuType _preType = menuType.NewGame;
    int _preIndex = 0;
    List<menuType> _menuList;

    [SerializeField] Text[] _menu;
    List<Vector3> _destVec;
    [SerializeField] GameObject _selectBar;

    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] string _nextSceneName;

    [Header("Font Size")]
    [SerializeField] int _selectFontSize = 50;
    [SerializeField] int _defaultFontSize = 45;

    bool _onMove;
    bool _onUp;
    bool _onDown;
    bool _onEnter;
    bool _doMove;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        _menuList = new List<menuType>();
        _menuList.Add(menuType.NewGame);
        _menuList.Add(menuType.Continue);
        _menuList.Add(menuType.Option);
        _menuList.Add(menuType.Exit);

        _destVec = new List<Vector3>();
        for (int i = 0; i < _menu.Length; i++)
            _destVec.Add(_menu[i].transform.localPosition);
    }

    void Update()
    {
        GetInput();
        MoveBar();
    }

    void GetInput()
    {
        _onUp = Input.GetButtonDown("Up");
        _onDown = Input.GetButtonDown("Down");
        _onEnter = Input.GetButtonDown("Submit");

        if (_onUp && !_onDown || !_onUp && _onDown)
            CheckDirection(_onUp);
        else if (_onEnter)
            Select();
    }

    void CheckDirection(bool up) // false is down
    {
        audioSource.Play();

        if (up)
        {
            _preIndex--;

            if (_preIndex < 0)
                _preIndex = _menuList.Count - 1;

            _preType = _menuList[_preIndex];
        }
        else
        {
            _preIndex++;

            if (_preIndex >= _menuList.Count)
                _preIndex = 0;

            _preType = _menuList[_preIndex];
        }

        ChangeFont();
    }

    void Select()
    {
        switch (_preType)
        {
            case menuType.NewGame:
                LoadingSceneController.LoadScene(_nextSceneName);
                break;
            case menuType.Continue:
                break;
            case menuType.Option:
                break;
            case menuType.Exit:
                Exit();
                break;
        }
    }

    void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void MoveBar()
    {
        _selectBar.transform.localPosition = 
            Vector3.Lerp(_selectBar.transform.localPosition, _destVec[_preIndex], Time.deltaTime * _moveSpeed);
    }

    void ChangeFont()
    {
        for (int i = 0; i < _menu.Length; i++)
        {
            if (i == _preIndex)
            {
                _menu[_preIndex].fontStyle = FontStyle.Bold;
                _menu[_preIndex].fontSize = _selectFontSize;
            }
            else
            {
                _menu[i].fontStyle = FontStyle.Normal;
                _menu[i].fontSize = _defaultFontSize;
            }
        }
    }
}
