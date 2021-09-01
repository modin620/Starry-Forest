using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectBar : MonoBehaviour
{
    [SerializeField] private RectTransform[] _selectPosition;
    [SerializeField] private Text[] _menu;
    [SerializeField] private float _moveSpeed;

    private RectTransform _rectTransform;
    private AudioSource _audioSource;

    private int _preIndex;
    private bool _onMove;
    private bool _onUp;
    private bool _onDown;
    private bool _onEnter;
    private bool _moveCooltime;

    private const int SELECT_FONT_SIZE = 50;
    private const int DEFAULT_FONT_SIZE = 45;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetInput();
        Select();
        CheckDirection();
        MoveBar();
    }

    private void GetInput()
    {
        _onUp = Input.GetButton("Up");
        _onDown = Input.GetButton("Down");
        _onEnter = Input.GetKeyDown(KeyCode.Return);

        if ((_onUp || _onDown) && !_moveCooltime)
        {
            _moveCooltime = true;
            _audioSource.Play();
            Invoke("WaitMove", 0.5f);
        }

    }

    private void Select()
    {
        if (!_onEnter)
            return;

        switch (_preIndex)
        {
            case 0:
                LoadingSceneController.LoadScene("StageScene");
                break;
            case 1:
            case 2:
            case 3:
                break;
        }
    }

    private void WaitMove()
    {
        _onMove = false;
        _moveCooltime = false;
    }

    private void CheckDirection()
    {
        if ((_onUp && _onDown) || (!_onUp && !_onDown))
            return;

        if (_onMove)
            return;

        _onMove = true;

        if (_onUp && !_onDown)
        {
            _preIndex--;
            if (_preIndex < 0) 
                _preIndex = 3;
        }
        else if (!_onUp && _onDown)
        {
            _preIndex++;
            _preIndex %= 4;
        }

        for (int i = 0; i < _menu.Length; i++)
        {
            if (i == _preIndex)
            {
                _menu[_preIndex].fontStyle = FontStyle.Bold;
                _menu[_preIndex].fontSize = SELECT_FONT_SIZE;
            }
            else
            {
                _menu[i].fontStyle = FontStyle.Normal;
                _menu[i].fontSize = DEFAULT_FONT_SIZE;
            }
        }
    }

    private void MoveBar()
    {
        Vector3 destVec = new Vector3(_rectTransform.anchoredPosition.x, _selectPosition[_preIndex].anchoredPosition.y, 0);
        _rectTransform.anchoredPosition = Vector3.Lerp(_rectTransform.anchoredPosition, destVec, Time.deltaTime * _moveSpeed);
    }
}
