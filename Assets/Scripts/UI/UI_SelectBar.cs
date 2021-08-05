using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SelectBar : MonoBehaviour
{
    [SerializeField] private RectTransform[] _selectPosition;
    [SerializeField] private float _moveSpeed;

    private RectTransform rectTransform;

    private int _preIndex;
    private bool _onMove;
    private bool _onUp;
    private bool _onDown;
    private bool _moveCooltime;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        GetInput();
        CheckDirection();
        MoveBar();
        Debug.Log("onMove: " + _onMove);
        Debug.Log("preIndex: " + _preIndex);
        Debug.Log("onUp: " + _onUp);
        Debug.Log("onDown: " + _onDown);
    }

    private void GetInput()
    {
        _onUp = Input.GetButton("Up");
        _onDown = Input.GetButton("Down");

        if ((_onUp || _onDown) && !_moveCooltime)
        {
            _moveCooltime = true;
            Invoke("WaitMove", 0.5f);
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

    }

    private void MoveBar()
    {
        Vector3 destVec = new Vector3(rectTransform.anchoredPosition.x, _selectPosition[_preIndex].anchoredPosition.y, 0);
        rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, destVec, Time.deltaTime * _moveSpeed);
    }
}
